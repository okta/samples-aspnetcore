using Microsoft.AspNetCore.Mvc;
using Okta.Idx.Sdk;
using okta_aspnetcore_mvc_example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Okta
{
    public interface IInteractionRequiredHandler
    {
        Task<IActionResult> HandleInteractionRequired(Controller callingController, IIdxClient idxClient, IdxContext idxContext, ErrorViewModel viewModel);
    }
}
