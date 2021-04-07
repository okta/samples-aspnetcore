using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOktaSignInWidgetConfigurationProvider oktaSignInWidgetConfigurationProvider;        

        public AccountController(IOktaSignInWidgetConfigurationProvider oktaSignInWidgetConfigurationProvider)
        {
            this.oktaSignInWidgetConfigurationProvider = oktaSignInWidgetConfigurationProvider;            
        }

        [HttpGet]
        public async Task<IActionResult> SignIn([FromQuery]string state = null)
        {
            return View(await oktaSignInWidgetConfigurationProvider.GetOktaSignInWidgetConfigurationAsync(state));
        }

        [HttpGet]
        public async Task<IActionResult> SignInWithHelper([FromQuery] string state = null)
        {
            return View(await oktaSignInWidgetConfigurationProvider.GetOktaSignInWidgetConfigurationAsync(state));
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {            
            return View(await HttpContext.GetOktaUserInfoAsync());
        }

        [HttpPost]
        public override SignOutResult SignOut()
        {
            return new SignOutResult(
                new[]
                {
                     CookieAuthenticationDefaults.AuthenticationScheme,
                },
                new AuthenticationProperties { RedirectUri = "/Home/" });
        }
    }
}