using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example
{
    public class OktaIdentity : IIdentity
    {
        public OktaIdentity(IIdentity identity, string name = null)
        {
            Identity = identity;
            Name = name ?? identity.Name;
        }

        public OktaIdentity(ClaimsPrincipal claimsPrincipal, string name): this(claimsPrincipal.Identity, name)
        {
            ClaimsPrincipal = claimsPrincipal;
        }

        protected ClaimsPrincipal ClaimsPrincipal{ get; set; }
        protected IIdentity Identity{ get; set; }

        string authenticationType;
        public string AuthenticationType
        {
            get
            {
                return authenticationType ?? Identity.AuthenticationType;
            }
            set
            {
                authenticationType = value;
            }
        }

        bool? isAuthenticated;
        public bool IsAuthenticated
        {
            get
            {
                return isAuthenticated ?? Identity.IsAuthenticated;
            }
            set
            {
                isAuthenticated = value;
            }
        }

        public string Name 
        {
            get;
            set;
        }
    }
}
