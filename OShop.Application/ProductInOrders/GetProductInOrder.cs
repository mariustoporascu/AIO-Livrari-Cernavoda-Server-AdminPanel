using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.ProductInOrders
{
    public class GetProductInOrder
    {
        private readonly OnlineShopDbContext _context;

        public GetProductInOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public ProductInOrdersViewModel Do(int orderId, int productId)
        {
            var productInOrder = _context.ProductInOrders.AsNoTracking().FirstOrDefault(productInOrder => productInOrder.OrderRefId == orderId && productInOrder.ProductRefId == productId);
            if (productInOrder != null)
                return new ProductInOrdersViewModel
                {
                    OrderRefId = productInOrder.OrderRefId,
                    ProductRefId = productInOrder.ProductRefId,
                    UsedQuantity = productInOrder.UsedQuantity,
                };
            else
                return null;
        }
    }
}
