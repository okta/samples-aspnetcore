using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public class OktaSignInWidgetConfiguration
    {
        public const string DefaultVersion = "5.5.4";

        public OktaSignInWidgetConfiguration(IdxConfiguration idxConfiguration, IIdxContext idxContext, string version = DefaultVersion)
        {
            this.UseInteractionCodeFlow = true;
            this.Version = version ?? DefaultVersion;

            this.BaseUrl = idxConfiguration?.Issuer?.Split("/oauth2")[0];
            this.ClientId = idxConfiguration.ClientId;
            this.RedirectUri = idxConfiguration.RedirectUri;
            this.AuthParams = new OktaSignInWidgetAuthParams(idxConfiguration);

            this.InteractionHandle = idxContext.InteractionHandle;
            this.State = idxContext.State;
            this.CodeChallenge = idxContext.CodeChallenge;
            this.CodeChallengeMethod = idxContext.CodeChallengeMethod;
        }

        [JsonProperty("interactionHandle")]
        public string InteractionHandle { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("redirectUri")]
        public string RedirectUri { get; set; }

        [JsonProperty("authParams")]
        public OktaSignInWidgetAuthParams AuthParams { get; set; }

        [JsonProperty("useInteractionCodeFlow")]
        public bool UseInteractionCodeFlow { get; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("codeChallenge")]
        public string CodeChallenge { get; set; }

        [JsonProperty("codeChallengeMethod")]
        public string CodeChallengeMethod { get; set; }
    }
}
