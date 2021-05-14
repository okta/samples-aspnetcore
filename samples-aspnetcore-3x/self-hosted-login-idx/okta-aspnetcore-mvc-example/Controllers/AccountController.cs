using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Okta.Idx.Sdk;
using okta_aspnetcore_mvc_example.Okta;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdxClient idxClient;

        public AccountController(IIdxClient idxClient)
        {
            this.idxClient = idxClient;
        }

        public async Task<IActionResult> SignInWidget()
        {
            SignInWidgetConfiguration signInWidgetConfiguration = await HttpContext.StartWidgetSignInAsync(idxClient);
            return View(signInWidgetConfiguration);
        }

        public async Task<IActionResult> Profile()
        {
            return View(HttpContext.User.Claims);
        }

        public async Task<IActionResult> SignOut()
        {
            var accessToken = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "access_token");
            if(accessToken != null)
            {
                await idxClient.RevokeTokensAsync(TokenType.AccessToken, accessToken.Value);
            }
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
