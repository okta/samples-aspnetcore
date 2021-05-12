using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using okta_social_login_example.Models;
using okta_social_login_example.Okta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace okta_social_login_example.Controllers
{
    public class InteractionCodeController : Controller
    {
        private readonly IIdxClient idxClient;
        private readonly IInteractionRequiredHandler interactionRequiredHandler;
        private readonly ILogger<InteractionCodeController> logger;        

        public InteractionCodeController(IIdxClient idxClient, IInteractionRequiredHandler interactionRequiredHandler, ILogger<InteractionCodeController> logger)
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
            IdxContext idxContext = HttpContext.Session.GetIdxContext(state);

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

        private async Task RedeemInteractionCodeAndSignInAsync(IdxContext idxContext, string interactionCode)
        {
            try
            {
                OktaTokens tokens = await idxClient.RedeemInteractionCodeAsync(idxContext, interactionCode, HandleException);

                if (tokens == null)
                {
                    // if the tokens object is null, our exception handler (HandleException) should have executed.
                    // The line below is included for completeness.
                    HttpContext.Response.Redirect("/");
                }
                else
                {
                    BearerToken bearerToken = new BearerToken(tokens.AccessToken);
                    Dictionary<string, object> claims = bearerToken.GetClaims();
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    foreach(string claimType in claims.Keys)
                    {
                        identity.AddClaim(new Claim(claimType, claims[claimType]?.ToString()));
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                HttpContext.Response.Redirect("/");
            }
        }

        private void HandleException(Exception ex)
        {
            logger.LogError(ex, ex.Message);

            // provide exception handling appropriate to your application
            HttpContext.Response.Redirect("/");
        }
    }
}
