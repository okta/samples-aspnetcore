using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example
{
    public class OktaPrincipal : ClaimsPrincipal
    {
        public OktaPrincipal(ClaimsPrincipal principal, OktaClientConfiguration oktaConfiguration)
        {
            groups = new HashSet<IGroup>();
            OktaClientConfiguration = oktaConfiguration;
            ClaimsPrincipal = principal;
            Identity = new OktaIdentity(principal, GetUserId(principal));
        }

        public OktaPrincipal(ClaimsPrincipal principal, IConfiguration configuration): this(principal, new OktaClientConfiguration
        {
            OktaDomain = configuration.GetValue<string>("Okta:OktaDomain"),
            Token = configuration.GetValue<string>("Okta:Token"),
        })
        {
        }

        protected OktaClientConfiguration OktaClientConfiguration{ get; set; }

        protected ClaimsPrincipal ClaimsPrincipal{ get; set; }

        public static string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims?.FirstOrDefault(claim => (bool)claim.Type?.Equals("sub")).Value;
        }

        public IIdentity Identity
        {
            get; private set;
        }

        HashSet<IGroup> groups;
        public bool IsInRole(string role)
        {
            return IsInRoleAsync(role).Result;
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            if(!groups.Any())
            {
                groups.Clear();
                string userId = GetUserId(ClaimsPrincipal);
                foreach(IGroup group in await new OktaClient(OktaClientConfiguration).Users.ListUserGroups(userId).ToListAsync())
                {
                    groups.Add(group);
                }
            }

            return groups.Any(group => (bool)group.Profile?.Name?.Equals(role));
        }
    }
}
