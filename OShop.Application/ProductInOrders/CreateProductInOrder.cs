using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class CreateProductInOrder
    {
        private readonly OnlineShopDbContext _context;

        public CreateProductInOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(List<ProductInOrder> vm)
        {
            foreach (var item in vm)
            {
                _context.ProductInOrders.Add(item);
            }

            await _context.SaveChangesAsync();
        }
    }

}
