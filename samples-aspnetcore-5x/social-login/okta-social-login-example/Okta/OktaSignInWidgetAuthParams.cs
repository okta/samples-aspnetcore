using Newtonsoft.Json;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public class OktaSignInWidgetAuthParams
    {
        public OktaSignInWidgetAuthParams()
        {
        }

        public OktaSignInWidgetAuthParams(IdxConfiguration idxConfiguration)
        {
            Issuer = idxConfiguration.Issuer;
            Scopes = idxConfiguration.Scopes;
        }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
    }
}
