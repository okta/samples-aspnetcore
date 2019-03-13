using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace okta_aspnetcore_webapi_example.Controllers
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    [Produces("application/json")]
    [Authorize]
    public class MessagesController : Controller
    {
        [HttpGet]
        [Route("~/api/messages")]
        [EnableCors("AllowAll")]
        public JsonResult Get()
        {
            var principal = HttpContext.User.Identity as ClaimsIdentity;

            var login = principal.Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            return Json(new
            {
                messages = new dynamic[]
                {
                    new { Date = DateTime.Now, Text = "I am a Robot." },
                    new { Date = DateTime.Now, Text = "Hello, world!" },
                },
            });
        }
    }
}
