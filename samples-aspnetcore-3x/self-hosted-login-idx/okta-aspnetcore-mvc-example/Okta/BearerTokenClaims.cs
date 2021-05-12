using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Okta
{
    /// <summary>
    /// Represents the claims within a bearer token (jwt).
    /// </summary>
    public class BearerTokenClaims
    {
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        [JsonProperty("iss")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        [JsonProperty("sub")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        [JsonProperty("aud")]
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        [JsonProperty("exp")]
        public string ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        [JsonProperty("scp")]
        public string[] Scope { get; set; }

        /// <summary>
        /// Gets claims from the specified bearer token.
        /// </summary>
        /// <param name="bearerToken">The bearer token.</param>
        /// <returns>BearerTokenClaims.</returns>
        public static BearerTokenClaims FromBearerToken(BearerToken bearerToken)
        {
            if (bearerToken?.Payload == null)
            {
                return new BearerTokenClaims();
            }

            return FromPayload(bearerToken.Payload);
        }

        /// <summary>
        /// Gets claims from the specified payload.
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <returns>BearerTokenClaims.</returns>
        public static BearerTokenClaims FromPayload(string payload)
        {
            if (string.IsNullOrEmpty(payload))
            {
                return new BearerTokenClaims();
            }

            return JsonConvert.DeserializeObject<BearerTokenClaims>(payload);
        }
    }
}
