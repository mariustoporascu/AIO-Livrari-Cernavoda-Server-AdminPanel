using OShop.Database;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class DeleteOrder
    {
        private readonly OnlineShopDbContext _context;

        public DeleteOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(string customerId)
        {
            var order = _context.Orders.FirstOrDefault(order => order.CustomerId == customerId);
            await _context.SaveChangesAsync();
        }

        public async Task Do(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == orderId);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
