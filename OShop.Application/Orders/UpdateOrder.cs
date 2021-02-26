using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class UpdateOrder
    {
        private readonly OnlineShopDbContext _context;

        public UpdateOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(OrderViewModel vm)
        {
            var order = new Order
            {
                OrderId = vm.OrderId,
                Status = vm.Status,
                CustomerId = vm.CustomerId,
                TotalOrdered = vm.TotalOrdered,
                Created = vm.Created,
            };
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
