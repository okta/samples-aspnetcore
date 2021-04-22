using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Models
{
    public class OktaUserInfo
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
        public int UpdatedAt { get; set; }
    }
}
