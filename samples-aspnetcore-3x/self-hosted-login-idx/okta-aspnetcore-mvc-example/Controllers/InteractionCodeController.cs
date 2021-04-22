using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IIdxClient idxClient;
        private readonly HttpClient httpClient;

        public InteractionCodeController(IIdxClient idxClient)
        {
            this.idxClient = idxClient;
            this.httpClient = new HttpClient();
        }

        public async Task<IActionResult> Callback()
        {
            await GetTokensAsync(HttpContext);
            return Redirect("/Account/Profile");
        }

        private async Task GetTokensAsync(HttpContext httpContext)
        {
            try
            {
                IdxConfiguration idxConfiguration = idxClient.Configuration;

                string state = httpContext.Request.Query["state"];
                string idxContextJson = httpContext.Session.GetString(state);

                Dictionary<string, string> idxContext = JsonConvert.DeserializeObject<Dictionary<string, string>>(idxContextJson);

                string interactionCode = httpContext.Request.Query["interaction_code"];
                Uri issuerUri = new Uri(idxConfiguration.Issuer);
                string domain = issuerUri.Authority;
                Uri tokenUri = new Uri(GetNormalizedUriString(issuerUri.ToString(), "v1/token")); 
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, tokenUri);

                StringBuilder requestContent = new StringBuilder();
                this.AddParameter(requestContent, "grant_type", "interaction_code", false);
                this.AddParameter(requestContent, "client_id", idxConfiguration.ClientId, true);
                if (!string.IsNullOrEmpty(idxConfiguration.ClientSecret))
                {
                    this.AddParameter(requestContent, "client_secret", idxConfiguration.ClientSecret, true);
                }

                this.AddParameter(requestContent, "interaction_code", interactionCode, true);
                this.AddParameter(requestContent, "code_verifier", idxContext["CodeVerifier"], true);

                requestMessage.Content = new StringContent(requestContent.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                requestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage);
                string tokenResponseJson = await responseMessage.Content.ReadAsStringAsync();
                if (!responseMessage.IsSuccessStatusCode)
                {
                    httpContext.Response.Redirect("/");
                }
                else
                {
                    OktaTokens tokens = JsonConvert.DeserializeObject<OktaTokens>(tokenResponseJson);
                    string oktaUserInfoJson = await GetUserInfoJsonAsync(idxConfiguration, tokens.AccessToken);
                    OktaUserInfo oktaUserInfo = JsonConvert.DeserializeObject<OktaUserInfo>(oktaUserInfoJson);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, oktaUserInfo.PreferredUserName));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, oktaUserInfo.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, oktaUserInfo.FamilyName));
                    
                    var principal = new ClaimsPrincipal(identity);
                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                httpContext.Response.Redirect("/");
            }
        }

        internal static async Task<string> GetUserInfoJsonAsync(IdxConfiguration idxConfiguration, string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            Uri issuerUri = new Uri(idxConfiguration.Issuer);
            Uri userInfoUri = new Uri(GetNormalizedUriString(issuerUri.ToString(), "v1/userinfo"));

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, userInfoUri);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            httpRequestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        private void AddParameter(StringBuilder stringBuilder, string key, string value, bool ampersandPrefix = false)
        {
            if (ampersandPrefix)
            {
                stringBuilder.Append("&");
            }

            stringBuilder.Append($"{key}={value}");
        }

        private static string GetNormalizedUriString(string issuer, string resourceUri)
        {
            string normalized = issuer;
            if (IsRootOrgIssuer(issuer))
            {
                normalized = Path.Combine(normalized, "oauth2", resourceUri);
            }
            else
            {
                normalized = Path.Combine(normalized, resourceUri);
            }
            return normalized;
        }

        private static bool IsRootOrgIssuer(string issuerUri)
        {
            string path = new Uri(issuerUri).AbsolutePath;
            string[] splitUri = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
            if(splitUri.Length >= 2 &&
            "oauth2".Equals(splitUri[0]) &&
            !string.IsNullOrEmpty(splitUri[1]))
            {
                return false;
            }

            return true;
        }
    }
}
