using Okta.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okta_aspnetcore_mvc_example.Models
{
    public class ProfileModel
    {
        public OktaUserInfo UserInfo { get; set; }
        public bool Debug { get; set; }
    }
}
