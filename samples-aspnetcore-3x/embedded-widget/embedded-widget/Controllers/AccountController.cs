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

        [HttpGet]
        public async Task<IActionResult> SignInWidget([FromQuery]string state = null)
        {
            SignInWidgetConfiguration signInWidgetConfiguration = await HttpContext.StartWidgetSignInAsync(idxClient, state);
            if(string.IsNullOrEmpty(state))
            {
                // redirect back to current action with state to allow reload without starting a new idx interaction
                return Redirect($"/Account/SignInWidget?state={signInWidgetConfiguration.State}");
            }
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
