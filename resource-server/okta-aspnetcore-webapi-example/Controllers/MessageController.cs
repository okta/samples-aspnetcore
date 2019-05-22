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
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("~/api/messages")]
        [EnableCors("AllowAll")]
        public JsonResult Get()
        {
            var principal = HttpContext.User.Identity as ClaimsIdentity;

            var login = principal.Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            return new JsonResult(new
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
