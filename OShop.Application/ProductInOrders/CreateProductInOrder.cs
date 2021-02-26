using OShop.Database;
using OShop.Domain.Models;
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

        public async Task Do(ProductInOrdersViewModel vm)
        {
            _context.ProductInOrders.Add(new ProductInOrder
            {
                OrderRefId = vm.OrderRefId,
                ProductRefId = vm.ProductRefId,
                UsedQuantity = vm.UsedQuantity,
            });
            await _context.SaveChangesAsync();
        }
    }

    public class ProductInOrdersViewModel
    {
        public int OrderRefId { get; set; }

        public int ProductRefId { get; set; }

        public int UsedQuantity { get; set; }
    }
}
