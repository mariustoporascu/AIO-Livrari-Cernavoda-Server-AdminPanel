using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class GetProductInOrder
    {
        private readonly ApplicationDbContext _context;

        public GetProductInOrder(ApplicationDbContext context)
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
