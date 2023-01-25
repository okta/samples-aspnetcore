using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace okta_aspnetcore_razorpages_example.Pages.Account
{
    public class SignInModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string sessionToken)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var properties = new AuthenticationProperties();
                properties.Items.Add("sessionToken", sessionToken);
                properties.RedirectUri = "/Index/";

                return Challenge(properties, "OpenIdConnect");
            }

            return Redirect("/Index");
        }
    }
}
