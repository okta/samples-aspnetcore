using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using okta_aspnetcore_mvc_example.Okta;
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

        public async Task<IActionResult> SignIn()
        {
            IIdxContext idxContext = await this.idxClient.InteractAsync();
            string idxContextJson = JsonConvert.SerializeObject(idxContext);
            HttpContext.Session.SetString(idxContext.State, idxContextJson);
            return View(new OktaSignInWidgetConfiguration(idxClient.Configuration, idxContext));
        }

        public async Task<IActionResult> Profile()
        {
            return View(HttpContext.User.Claims);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}