using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class UpdateProductInOrder
    {
        private readonly ApplicationDbContext _context;

        public UpdateProductInOrder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(ProductInOrdersViewModel vm)
        {
            var productInOrder = new ProductInOrder
            {
                OrderRefId = vm.OrderRefId,
                ProductRefId = vm.ProductRefId,
                UsedQuantity = vm.UsedQuantity,
            };
            _context.ProductInOrders.Update(productInOrder);
            await _context.SaveChangesAsync();
        }
    }
}
