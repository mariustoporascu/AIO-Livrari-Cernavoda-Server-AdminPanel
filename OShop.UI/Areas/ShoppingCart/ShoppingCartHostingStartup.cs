using Microsoft.AspNetCore.Hosting;


[assembly: HostingStartup(typeof(OShop.UI.Areas.ShoppingCart.ShoppingCartHostingStartup))]
namespace OShop.UI.Areas.ShoppingCart
{
    public class ShoppingCartHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}