using Microsoft.AspNetCore.Hosting;


[assembly: HostingStartup(typeof(OShop.UI.Areas.Identity.IdentityHostingStartup))]
namespace OShop.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}