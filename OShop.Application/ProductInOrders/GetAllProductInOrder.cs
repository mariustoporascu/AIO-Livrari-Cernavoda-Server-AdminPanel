using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class GetAllProductInOrder
    {
        private readonly OnlineShopDbContext _context;

        public GetAllProductInOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductInOrdersViewModel> Do(int orderId) => 
            _context.ProductInOrders.AsNoTracking().Where(productInOrder => productInOrder.OrderRefId == orderId)
                .Select(productInOrder => new ProductInOrdersViewModel
                {
                    OrderRefId = productInOrder.OrderRefId,
                    ProductRefId = productInOrder.ProductRefId,
                    UsedQuantity = productInOrder.UsedQuantity,
                });
    }
}
