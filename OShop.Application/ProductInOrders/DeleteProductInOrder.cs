using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class DeleteProductInOrder
    {
        private readonly OnlineShopDbContext _context;

        public DeleteProductInOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(int orderId, int productId)
        {
            var productInOrder = new ProductInOrder
            {
                OrderRefId = orderId,
                ProductRefId = productId,
            };
            _context.ProductInOrders.Remove(productInOrder);
            await _context.SaveChangesAsync();
        }
    }
}
