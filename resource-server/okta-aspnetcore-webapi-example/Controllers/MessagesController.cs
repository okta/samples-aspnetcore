using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace okta_aspnetcore_webapi_example.Controllers
{
    [Produces("application/json")]
    [Authorize]
    public class MessagesController : Controller
    {
        [HttpGet]
        [Route("~/api/messages")]
        public JsonResult Get()
        {
            var principal = HttpContext.User.Identity as ClaimsIdentity;

            var login = principal.Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            
            return Json( new
            {
                messages = new dynamic[]
                {
                    new { Date = DateTime.Now, Text = "I am a Robot." },
                    new { Date = DateTime.Now, Text = "Hello, world!" },
                }
            });
        }
    }
}