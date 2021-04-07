using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Models
{
    public abstract class IdxModel
    {
        public IIdxClient IdxClient { get; set; }
        public IIdxContext IdxContext { get; set; }
        public IdxConfiguration IdxConfiguration { get; set; }
    }
}
