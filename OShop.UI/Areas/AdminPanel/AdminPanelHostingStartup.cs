using Microsoft.AspNetCore.Hosting;


[assembly: HostingStartup(typeof(OShop.UI.Areas.AdminPanel.AdminPanelHostingStartup))]
namespace OShop.UI.Areas.AdminPanel
{
    public class AdminPanelHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}