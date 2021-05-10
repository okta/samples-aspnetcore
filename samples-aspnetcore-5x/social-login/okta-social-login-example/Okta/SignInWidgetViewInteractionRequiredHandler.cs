using Microsoft.AspNetCore.Mvc;
using Okta.Idx.Sdk;
using okta_social_login_example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public class SignInWidgetViewInteractionRequiredHandler : IInteractionRequiredHandler
    {
        public async Task<IActionResult> HandleInteractionRequired(Controller callingController, IIdxClient idxClient, IdxContext idxContext, ErrorViewModel viewModel)
        {
            return callingController.View("SignInWidget", new OktaSignInWidgetConfiguration(idxClient.Configuration, idxContext));
        }
    }
}
