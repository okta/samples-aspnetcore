using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Okta.AspNetCore;

namespace okta_aspnetcore_razorpages_example.Pages.Account
{
    public class SignOutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            return new SignOutResult(
                new[]
                {
                     OktaDefaults.MvcAuthenticationScheme,
                     CookieAuthenticationDefaults.AuthenticationScheme,
                },
                new AuthenticationProperties { RedirectUri = "/Index" });
        }
    }
}
