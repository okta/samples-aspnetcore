using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Idx = Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using okta_aspnetcore_mvc_example.Models;
using ExampleApp = okta_aspnetcore_mvc_example.Okta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Controllers
{
    public class InteractionCodeController : Controller
    {
        private readonly Idx.IIdxClient idxClient;
        private readonly ExampleApp.IInteractionRequiredHandler interactionRequiredHandler;
        private readonly ILogger<InteractionCodeController> logger;

        public InteractionCodeController(Idx.IIdxClient idxClient, ExampleApp.IInteractionRequiredHandler interactionRequiredHandler, ILogger<InteractionCodeController> logger)
        {
            this.idxClient = idxClient;
            this.interactionRequiredHandler = interactionRequiredHandler;
            this.logger = logger;
        }

        public async Task<IActionResult> Callback(
            [FromQuery(Name = "state")] string state = null,
            [FromQuery(Name = "interaction_code")] string interactionCode = null,
            [FromQuery(Name = "error")] string error = null,
            [FromQuery(Name = "error_description")] string errorDescription = null)
        {
            Idx.IdxContext idxContext = ExampleApp.OktaExtensions.GetIdxContext(HttpContext.Session, state);

            if ("interaction_required".Equals(error))
            {
                return await interactionRequiredHandler.HandleInteractionRequired(this, idxClient, idxContext, new ErrorViewModel { Error = error, ErrorDescription = errorDescription });
            }

            if (!string.IsNullOrEmpty(error))
            {
                return View("Error", new ErrorViewModel { Error = error, ErrorDescription = errorDescription });
            }

            if (string.IsNullOrEmpty(interactionCode))
            {
                return View("Error", new ErrorViewModel { Error = "null_interaction_code", ErrorDescription = "interaction_code was not specified" });
            }

            await RedeemInteractionCodeAndSignInAsync(idxContext, interactionCode);
            return Redirect("/Home/Profile");
        }

        private async Task RedeemInteractionCodeAndSignInAsync(Idx.IdxContext idxContext, string interactionCode)
        {
            try
            {
                Idx.TokenResponse tokens = await idxClient.RedeemInteractionCodeAsync(idxContext, interactionCode);

                if (tokens == null)
                {
                    // if the tokens object is null, our exception handler (HandleException) should have executed.
                    // The line below is included for completeness.
                    HttpContext.Response.Redirect("/");
                }
                else
                {
                    ClaimsPrincipal principal = await GetClaimsPrincipalAsync(tokens);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                HttpContext.Response.Redirect("/");
            }
        }

        private async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(Idx.TokenResponse tokens)
        {           
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = await GetClaimsFromUserInfoAsync(tokens.AccessToken);
            identity.AddClaims(claims);
            identity.AddClaim(new Claim("access_token", tokens.AccessToken));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return principal;
        }

        private async Task<IEnumerable<Claim>> GetClaimsFromUserInfoAsync(string accessToken)
        {
            Uri userInfoUri = new Uri(Idx.IdxUrlHelper.GetNormalizedUriString(idxClient.Configuration.Issuer, "v1/userinfo")) ;
            HttpClient httpClient = new HttpClient();
            UserInfoResponse userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = userInfoUri.ToString(),
                Token = accessToken,
            }).ConfigureAwait(false);

            return userInfoResponse.Claims;
        }

        private void HandleException(Exception ex)
        {
            logger.LogError(ex, ex.Message);

            // provide exception handling appropriate to your application
            HttpContext.Response.Redirect("/");
        }
    }
}
