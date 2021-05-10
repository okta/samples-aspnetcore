// <copyright file="BearerTokenClaims.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace okta_social_login_example.Models
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
