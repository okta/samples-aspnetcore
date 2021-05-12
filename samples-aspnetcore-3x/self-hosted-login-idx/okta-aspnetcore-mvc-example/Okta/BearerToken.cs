using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Okta
{
    /// <summary>
    /// Represents a parsed Jwt (json web token) bearer token.
    /// </summary>
    public class BearerToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BearerToken"/> class.
        /// </summary>
        /// <param name="jwtToken">The jwt token to parse.</param>
        public BearerToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                return;
            }

            string[] segments = jwtToken.Split('.');
            if (segments.Length != 3)
            {
                return;
            }

            this.Base64UrlEncodedHeader = segments[0];
            this.Base64UrlEncodedPayload = segments[1];
            this.Signature = segments[2];
        }

        /// <summary>
        /// Gets the base64 url encoded header.
        /// </summary>
        public string Header
        {
            get
            {
                return string.IsNullOrEmpty(this.Base64UrlEncodedHeader) ? string.Empty : Base64UrlEncoder.Decode(this.Base64UrlEncodedHeader);
            }
        }

        /// <summary>
        /// Gets the base 64 url encoded payload.
        /// </summary>
        public string Payload
        {
            get
            {
                return string.IsNullOrEmpty(this.Base64UrlEncodedPayload) ? string.Empty : Base64UrlEncoder.Decode(this.Base64UrlEncodedPayload);
            }
        }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the base 64 url encoded header.
        /// </summary>
        protected string Base64UrlEncodedHeader { get; set; }

        /// <summary>
        /// Gets or sets the base 64 url encoded payload.
        /// </summary>
        protected string Base64UrlEncodedPayload { get; set; }

        /// <summary>
        /// Returns the base64 url encoded string representation of this BearerToken.
        /// </summary>
        /// <returns>jwt as string.</returns>
        public override string ToString()
        {
            return $"{this.Base64UrlEncodedHeader}.{this.Base64UrlEncodedPayload}.{this.Signature}";
        }

        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <returns>Dictionary.<string, object></returns>
        public Dictionary<string, object> GetClaims()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(BearerTokenClaims.FromBearerToken(this)));
        }
    }
}
