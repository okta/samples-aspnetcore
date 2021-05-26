using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Idx = Okta.Idx.Sdk;
using okta_aspnetcore_mvc_example.Models;
using ExampleApp = okta_aspnetcore_mvc_example.Okta;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Helpers;

namespace okta_aspnetcore_mvc_example.Controllers
{
    public class InteractionCodeController : Controller
    {
        private readonly Idx.IIdxClient idxClient;
        private readonly ILogger<InteractionCodeController> logger;

        public InteractionCodeController(Idx.IIdxClient idxClient, ILogger<InteractionCodeController> logger)
        {
            this.idxClient = idxClient;
            this.logger = logger;
        }

        public async Task<IActionResult> Callback(
            [FromQuery(Name = "state")] string state = null,
            [FromQuery(Name = "interaction_code")] string interactionCode = null,
            [FromQuery(Name = "error")] string error = null,
            [FromQuery(Name = "error_description")] string errorDescription = null)
        {
            try
            {
                Idx.IIdxContext idxContext = ExampleApp.OktaExtensions.GetIdxContext(HttpContext.Session, state);

                if ("interaction_required".Equals(error))
                {
                    return View("SignInWidget", new SignInWidgetConfiguration(idxClient.Configuration, idxContext));
                }

                if (!string.IsNullOrEmpty(error))
                {
                    return View("Error", new ErrorViewModel { Error = error, ErrorDescription = errorDescription });
                }

                if (string.IsNullOrEmpty(interactionCode))
                {
                    return View("Error", new ErrorViewModel { Error = "null_interaction_code", ErrorDescription = "interaction_code was not specified" });
                }

                Idx.TokenResponse tokens = await idxClient.RedeemInteractionCodeAsync(idxContext, interactionCode);
                ClaimsPrincipal principal = await AuthenticationHelper.GetPrincipalFromTokenResponseAsync(idxClient.Configuration, tokens);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Redirect("/Home/Profile");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel { Error = ex.GetType().Name, ErrorDescription = ex.Message });
            }
        }
    }
}
