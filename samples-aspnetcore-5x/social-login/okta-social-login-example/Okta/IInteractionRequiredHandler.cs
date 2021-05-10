using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Okta.Idx.Sdk;
using okta_social_login_example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Okta
{
    public interface IInteractionRequiredHandler
    {
        Task<IActionResult> HandleInteractionRequired(Controller callingController, IIdxClient idxClient, IdxContext idxContext, ErrorViewModel viewModel);
    }
}
