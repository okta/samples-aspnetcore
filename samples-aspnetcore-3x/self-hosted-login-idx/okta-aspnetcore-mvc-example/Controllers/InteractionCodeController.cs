using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using okta_aspnetcore_mvc_example.Models;
using okta_aspnetcore_mvc_example.Okta;
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
        private readonly HttpClient httpClient;
        private readonly IIdxClient idxClient;
        private readonly IInteractionRequiredHandler interactionRequiredHandler;
        private readonly ILogger<InteractionCodeController> logger;

        public InteractionCodeController(IIdxClient idxClient, IInteractionRequiredHandler interactionRequiredHandler, ILogger<InteractionCodeController> logger)
        {
            this.httpClient = new HttpClient();
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
                TokenResponse tokens = await idxClient.RedeemInteractionCodeAsync(idxContext, interactionCode);

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

        private async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(TokenResponse tokens)
        {
            UserInfo userInfo = await GetUserInfoAsync(idxClient, tokens.AccessToken);            
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(userInfo.ToClaims());
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return principal;
        }

        private void HandleException(Exception ex)
        {
            logger.LogError(ex, ex.Message);

            // provide exception handling appropriate to your application
            HttpContext.Response.Redirect("/");
        }

        private async Task<UserInfo> GetUserInfoAsync(IIdxClient idxClient, string accessToken)
        {
            IdxConfiguration idxConfiguration = idxClient.Configuration;
            Uri issuerUri = new Uri(idxConfiguration.Issuer);
            Uri userInfoUri = new Uri(IdxUrlHelper.GetNormalizedUriString(issuerUri.ToString(), "v1/userinfo"));
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, userInfoUri);
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
            HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage);
            string userInfoJson = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
        }
    }
}
