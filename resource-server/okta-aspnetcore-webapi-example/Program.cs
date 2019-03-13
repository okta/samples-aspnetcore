using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace okta_aspnetcore_webapi_example
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
