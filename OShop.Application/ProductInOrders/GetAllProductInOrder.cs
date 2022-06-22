using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Application.ProductInOrders
{
    public class GetAllProductInOrder
    {
        private readonly OnlineShopDbContext _context;

        public GetAllProductInOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductInOrder> Do(int orderId) =>
            _context.ProductInOrders.AsNoTracking().AsEnumerable().Where(productInOrder => productInOrder.OrderRefId == orderId);
    }
}
