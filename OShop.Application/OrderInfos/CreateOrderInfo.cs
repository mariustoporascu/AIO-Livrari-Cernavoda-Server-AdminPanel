using OShop.Database;
using OShop.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OShop.Application.OrderInfos
{
    public class CreateOrderInfo
    {
        private readonly OnlineShopDbContext _context;

        public CreateOrderInfo(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(OrderInfo vm)
        {
            _context.OrdersInfos.Add(vm);
            await _context.SaveChangesAsync();
        }
    }

}
