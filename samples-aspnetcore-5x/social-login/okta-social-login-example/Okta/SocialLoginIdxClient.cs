using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using Okta.Sdk.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public class SocialLoginIdxClient : IdxClient, ISocialLoginIdxClient 
    {
        private HttpClient httpClient;
        private ILogger logger;

        /// <summary>
        /// The key used to store the state handle in session.
        /// </summary>
        public const string IdxStateKey = "IdxStateHandle";

        public SocialLoginIdxClient(IdxConfiguration configuration = null,
            HttpClient httpClient = null,
            ILogger logger = null) : base(configuration, httpClient, logger)
        {
            this.httpClient = httpClient ?? DefaultHttpClient.Create
            (
                connectionTimeout: null,
                proxyConfiguration: null,
                logger: logger ?? NullLogger.Instance
            );

            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<SocialLoginSettings> StartSocialLoginAsync(HttpContext httpContext, string state = null, CancellationToken cancellationToken = default)
        {
            return await StartSocialLoginAsync(httpContext.Session, state);
        }

        /// <inheritdoc />
        public async Task<SocialLoginSettings> StartSocialLoginAsync(ISession session, string state = null, CancellationToken cancellationToken = default)
        {
            IIdxContext idxContext = await this.InteractAsync(state);
            IIdxResponse introspectResponse = await this.IntrospectAsync(idxContext);

            SocialLoginSettings socialLoginSettings = new SocialLoginSettings
            {
                Context = idxContext,
                IdpOptions = introspectResponse.Remediation?.RemediationOptions?
                    .Where(remediationOption => remediationOption.Name.Equals("redirect-idp"))
                    .Select(remediationOption => new IdpOption { State = idxContext.State, InteractionHandle = idxContext.InteractionHandle, Id = remediationOption.Idp.Id, Name = remediationOption.Idp.Name })
                    .ToArray(),
                Client = this,
                Configuration = this.Configuration,
            };

            session.SetString(IdxStateKey, idxContext.State);
            string contextJson = JsonConvert.SerializeObject(idxContext);
            session.SetString(idxContext.State, contextJson);

            return socialLoginSettings;
        }

        public async Task<string> GetIdpUrlAsync(IdxContext idxContext, string idpId)
        {
            IIdxResponse introspectResponse = await this.IntrospectAsync(idxContext.InteractionHandle);
            return introspectResponse.Remediation?.RemediationOptions?
                .Where(remediationOption => remediationOption.Name.Equals("redirect-idp") && (bool)remediationOption.Idp?.Id?.Equals(idpId))
                .Select(remediationOption => remediationOption.Href)
                .FirstOrDefault();
        }

        public async Task<OktaTokens> RedeemInteractionCodeAsync(IdxContext idxContext, string interactionCode, Action<Exception> exceptionHandler = null, CancellationToken cancellationToken = default)
        {
            exceptionHandler = exceptionHandler ?? LogError;
            try
            {
                Uri issuerUri = new Uri(Configuration.Issuer);
                Uri tokenUri = new Uri(GetNormalizedUriString(issuerUri.ToString(), "v1/token"));
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, tokenUri);

                StringBuilder requestContent = new StringBuilder();
                this.AddParameter(requestContent, "grant_type", "interaction_code", false);
                this.AddParameter(requestContent, "client_id", Configuration.ClientId, true);
                if (!string.IsNullOrEmpty(Configuration.ClientSecret))
                {
                    this.AddParameter(requestContent, "client_secret", Configuration.ClientSecret, true);
                }

                this.AddParameter(requestContent, "interaction_code", interactionCode, true);
                this.AddParameter(requestContent, "code_verifier", idxContext.CodeVerifier, true);

                requestMessage.Content = new StringContent(requestContent.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                requestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage);
                string tokenResponseJson = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    exceptionHandler(new RedeemInteractionCodeException(tokenResponseJson));
                }

                return JsonConvert.DeserializeObject<OktaTokens>(tokenResponseJson);
            }
            catch (Exception exception)
            {
                exceptionHandler(new RedeemInteractionCodeException(exception));
            }
            return null;
        }

        public async Task<OktaUserInfo> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            Uri issuerUri = new Uri(Configuration.Issuer);
            Uri userInfoUri = new Uri(GetNormalizedUriString(issuerUri.ToString(), "v1/userinfo"));

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, userInfoUri);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            httpRequestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            string userJson = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OktaUserInfo>(userJson);
        }

        protected async Task<IdpOption[]> GetIdpOptions(IdxContext idxContext)
        {
            IIdxResponse introspectResponse = await this.IntrospectAsync(idxContext);

            return introspectResponse.Remediation?.RemediationOptions?
                        .Where(remediationOption => remediationOption.Name.Equals("redirect-idp"))
                        .Select(remediationOption => new IdpOption { Id = remediationOption.Idp.Id, Name = remediationOption.Idp.Name })
                        .ToArray();
        }

        protected virtual void LogError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
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
            if (splitUri.Length >= 2 &&
            "oauth2".Equals(splitUri[0]) &&
            !string.IsNullOrEmpty(splitUri[1]))
            {
                return false;
            }

            return true;
        }
    }
}
