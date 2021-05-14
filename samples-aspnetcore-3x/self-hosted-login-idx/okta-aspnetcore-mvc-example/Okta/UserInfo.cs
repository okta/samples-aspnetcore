using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Okta
{
    public class UserInfo
    {
        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("preferred_username")]
        public string PreferredUserName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("zoneinfo")]
        public string ZoneInfo { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        public List<Claim> ToClaims()
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, GivenName, ClaimValueTypes.String),
                new Claim(ClaimTypes.Surname, FamilyName, ClaimValueTypes.String),
                new Claim("zoneinfo", ZoneInfo),
                new Claim("preferred_username", PreferredUserName, ClaimValueTypes.String),
                new Claim("given_name", GivenName, ClaimValueTypes.String),
                new Claim("family_name", FamilyName, ClaimValueTypes.String),
                new Claim("updated_at", UpdatedAt, ClaimValueTypes.String),
                new Claim(ClaimTypes.Sid, Sub, ClaimValueTypes.String),
            };
        }
    }
}
