using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.Application.ProductInOrders
{
    public class UpdateProductInOrder
    {
        private readonly OnlineShopDbContext _context;

        public UpdateProductInOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(ProductInOrder vm)
        {
            _context.ProductInOrders.Update(vm);
            await _context.SaveChangesAsync();
        }
    }
}
