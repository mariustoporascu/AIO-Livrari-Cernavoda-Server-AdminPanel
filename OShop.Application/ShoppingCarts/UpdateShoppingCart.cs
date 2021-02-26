using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.Application.ShoppingCarts
{
    public class UpdateShoppingCart
    {
        private readonly OnlineShopDbContext _context;

        public UpdateShoppingCart(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(ShoppingCartViewModel vm)
        {
            var cart = new ShoppingCart
            {
                CartId = vm.CartId,
                Status = vm.Status,
                CustomerId = vm.CustomerId,
                TotalInCart = vm.TotalInCart,
                Created = vm.Created,
            };
            _context.ShoppingCarts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTotal(int cartId, int quantity, decimal price)
        {
            var cart = new GetShoppingCart(_context).Do(cartId);
            cart.TotalInCart += price * quantity;
            await this.Do(cart);
        }
    }
}
