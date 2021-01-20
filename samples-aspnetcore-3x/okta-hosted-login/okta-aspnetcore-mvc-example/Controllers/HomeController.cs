using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using okta_aspnetcore_mvc_example.Models;

namespace okta_aspnetcore_mvc_example.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Groups()
        {
            OktaClientConfiguration config = new OktaClientConfiguration
            {
                OktaDomain = _configuration.GetValue<string>("Okta:OktaDomain"),
                Token = _configuration.GetValue<string>("Okta:Token"),
            };
            string userId = HttpContext.User.Claims.FirstOrDefault(c => (bool)c.Type?.Equals("sub")).Value;
            List<IGroup> groups = await new OktaClient(config).Users.ListUserGroups(userId).ToListAsync();
            return View(groups);
        }
        
        [Authorize]
        public IActionResult Profile()
        {
            return View(HttpContext.User.Claims);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
