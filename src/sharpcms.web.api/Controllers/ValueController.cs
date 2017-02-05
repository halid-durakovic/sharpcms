using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sharpcms.web.api.Controllers
{
    [Route("api/[controller]")]
    public class ValueController : Controller
    {
        [HttpGet("unsecured")]
        public IActionResult UnsecuredGet()
        {
            var currentUser = HttpContext.User;

            var currentClaims = currentUser.Claims;

            foreach (var claim in currentClaims)
            {
                Debug.WriteLine($"Claim -> {claim.Type} :: {claim.Value}");
            }

            return Ok(new List<string>() { "UnsecuredThingme" });
        }

        [Authorize]
        [HttpGet("secured")]
        public IActionResult SecuredGet()
        {
            return Ok(new List<string>() { "SecuredThingme" });
        }
    }
}