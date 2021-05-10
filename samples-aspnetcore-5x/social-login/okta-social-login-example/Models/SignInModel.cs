using Microsoft.AspNetCore.Http;
using Okta.Idx.Sdk;
using okta_social_login_example.Okta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Models
{
    public class SignInModel
    {
        public SignInModel(bool debug = false)
        {
            this.Debug = debug;
        }

        public ISocialLoginIdxClient IdxClient { get; set; }

        /// <summary>
        /// Gets sign in widget configuration.
        /// </summary>
        public OktaSignInWidgetConfiguration WidgetConfiguration
        {
            get => new OktaSignInWidgetConfiguration(IdxClient.Configuration, SocialLoginSettings.Context);
        }

        public SocialLoginSettings SocialLoginSettings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Debug { get; set; }
    }
}
