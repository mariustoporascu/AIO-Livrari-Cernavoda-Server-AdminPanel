using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.Application.CartItemsA
{
    public class CreateCartItem
    {
        private readonly OnlineShopDbContext _context;

        public CreateCartItem(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(CartItemsViewModel vm)
        {
            _context.CartItems.Add(new CartItems
            {
                CartRefId = vm.CartRefId,
                ProductRefId = vm.ProductRefId,
                Quantity = vm.Quantity,
            });
            await _context.SaveChangesAsync();
        }
    }
    public class CartItemsViewModel
    {
        public int CartRefId { get; set; }
        public int ProductRefId { get; set; }
        public int Quantity { get; set; }
    }
}
