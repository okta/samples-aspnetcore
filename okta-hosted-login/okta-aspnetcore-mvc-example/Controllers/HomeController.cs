using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using okta_aspnetcore_mvc_example.Models;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace okta_aspnetcore_mvc_example.Controllers
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View(HttpContext.User.Claims);
        }
    }
}
