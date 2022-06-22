using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
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

        public ProductInOrder Do(int orderId, int productId)
        {
            var productInOrder = _context.ProductInOrders.AsNoTracking().FirstOrDefault(productInOrder => productInOrder.OrderRefId == orderId && productInOrder.ProductRefId == productId);
            if (productInOrder != null)
                return productInOrder;
            else
                return null;
        }
    }
}
