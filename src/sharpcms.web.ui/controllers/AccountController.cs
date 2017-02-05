using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sharpcms.web.ui.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public IActionResult Login()
        {
            return Redirect("/");
        }

        public async Task<IActionResult> Logout()
        {
            var u = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name");

            if (u != null)
            {
                var userName = u.Value;

                await HttpContext.Authentication.SignOutAsync("Cookies");

                var redirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";

                return Redirect($"http://localhost:5000/account/logout?logoutId={userName}&redirectUrl={redirectUrl}");
            }

            return Redirect("/");
        }
    }
}