using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class DeleteProductInOrder
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductInOrder(ApplicationDbContext context)
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
