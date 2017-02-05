using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sharpcms.web.ui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var currentUser = HttpContext.User;

            var currentClaims = currentUser.Claims;

            foreach (var claim in currentClaims)
            {
                Debug.WriteLine($"Claim -> {claim.Type} :: {claim.Value}");
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Secure()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
