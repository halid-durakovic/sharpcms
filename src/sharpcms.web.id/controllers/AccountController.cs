using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using sharpcms.security;
using sharpcms.web.id.models.account;
using sharpcms.web.id.services.account;

namespace sharpcms.web.id.Controllers
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly AccountService _account;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IUserStore _users;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IHttpContextAccessor httpContextAccessor,
            IUserStore users)
        {
            _users = users;
            _interaction = interaction;
            _account = new AccountService(interaction, httpContextAccessor, clientStore);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await _account.BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
                return await ExternalLogin(vm.ExternalProviders.First().AuthenticationScheme, returnUrl);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (_users.ValidateCredentials(model.Username, model.Password))
                {
                    AuthenticationProperties props = null;

                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    ;

                    var user = _users.FindByUsername(model.Username);

                    await HttpContext.Authentication.SignInAsync(user.SubjectId, user.Username, props);

                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    return Redirect("~/");
                }

                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            var vm = await _account.BuildLoginViewModelAsync(model);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId, string redirectUrl)
        {
            var vm = await _account.BuildLogoutViewModelAsync(logoutId, redirectUrl);

            if (vm.ShowLogoutPrompt == false)
                return await Logout(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);

            if (model is LogoutViewModel)
                vm.PostLogoutRedirectUri = (model as LogoutViewModel).PostLogoutRedirectUrl;

            if (vm.TriggerExternalSignout)
            {
                var url = Url.Action("Logout", new {logoutId = vm.LogoutId});

                await HttpContext.Authentication.SignOutAsync(vm.ExternalAuthenticationScheme,
                    new AuthenticationProperties {RedirectUri = url});
            }

            await HttpContext.Authentication.SignOutAsync();

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            returnUrl = Url.Action("ExternalLoginCallback", new {returnUrl});

            if (AccountOptions.WindowsAuthenticationSchemes.Contains(provider))
            {
                if (HttpContext.User is WindowsPrincipal)
                {
                    var props = new AuthenticationProperties();

                    props.Items.Add("scheme", HttpContext.User.Identity.AuthenticationType);

                    var id = new ClaimsIdentity(provider);

                    id.AddClaim(new Claim(ClaimTypes.NameIdentifier, HttpContext.User.Identity.Name));

                    id.AddClaim(new Claim(ClaimTypes.Name, HttpContext.User.Identity.Name));

                    await HttpContext.Authentication.SignInAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme, new ClaimsPrincipal(id), props);

                    return Redirect(returnUrl);
                }
                else
                {
                    return new ChallengeResult(AccountOptions.WindowsAuthenticationSchemes);
                }
            }

            var props1 = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                Items = {{"scheme", provider}}
            };

            return new ChallengeResult(provider, props1);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var tempUser = info?.Principal;

            if (tempUser == null)
                throw new Exception("External authentication error");

            var claims = tempUser.Claims.ToList();

            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);

            if (userIdClaim == null)
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("Unknown userid");

            claims.Remove(userIdClaim);

            var provider = info.Properties.Items["scheme"];

            var userId = userIdClaim.Value;

            var user = _users.FindByExternalProvider(provider, userId);

            if (user == null)
                user = _users.AutoProvisionUser(provider, userId, claims);

            var additionalClaims = new List<Claim>();

            var sid = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);

            if (sid != null)
                additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));

            AuthenticationProperties props = null;

            var id_token = info.Properties.GetTokenValue("id_token");

            if (id_token != null)
            {
                props = new AuthenticationProperties();

                props.StoreTokens(new[] {new AuthenticationToken {Name = "id_token", Value = id_token}});
            }

            await HttpContext.Authentication.SignInAsync(user.SubjectId, user.Username, provider, props, additionalClaims.ToArray());

            await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            if (_interaction.IsValidReturnUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("~/");
        }
    }
}