using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.Application.OrderInfos
{
    public class UpdateOrderInfo
    {
        private readonly OnlineShopDbContext _context;

        public UpdateOrderInfo(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(OrderInfo vm)
        {
            _context.OrdersInfos.Update(vm);
            await _context.SaveChangesAsync();
        }
    }
}
