using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using global::Okta.Idx.Sdk;
    using global::Okta.Idx.Sdk.Configuration;
    using global::Okta.Idx.Sdk.Helpers;
    using IdentityModel.Client;
    using Microsoft.AspNet.Identity;
    public static class AuthenticationHelper
    {
        public static async Task<ClaimsPrincipal> GetPrincipalFromTokenResponseAsync(IdxConfiguration configuration, ITokenResponse tokenResponse)
        {
            var claims = await GetClaimsFromUserInfoAsync(configuration, tokenResponse.AccessToken);
            claims = claims.Append(new Claim("access_token", tokenResponse.AccessToken));
            claims = claims.Append(new Claim("id_token", tokenResponse.IdToken));
            if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
            {
                claims = claims.Append(new Claim("refresh_token", tokenResponse.RefreshToken));
            }
            ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return principal;
        }

        public static async Task<IEnumerable<Claim>> GetClaimsFromUserInfoAsync(IdxConfiguration configuration, string accessToken)
        {
            Uri userInfoUri = new Uri(IdxUrlHelper.GetNormalizedUriString(configuration.Issuer, "v1/userinfo"));
            HttpClient httpClient = new HttpClient();
            var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = userInfoUri.ToString(),
                Token = accessToken,
            }).ConfigureAwait(false);
            var nameClaim = new Claim(
                ClaimTypes.Name,
                userInfoResponse.Claims.FirstOrDefault(x => x.Type == "name")?.Value);
            return userInfoResponse.Claims.Append(nameClaim);
        }
    }
}
