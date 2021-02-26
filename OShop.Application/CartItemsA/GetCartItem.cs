using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.CartItemsA
{
    public class GetCartItem
    {
        private readonly OnlineShopDbContext _context;

        public GetCartItem(OnlineShopDbContext context)
        {
            _context = context;
        }

        public CartItemsViewModel Do(int cartId, int productId)
        {
            var cart = _context.CartItems.AsNoTracking().FirstOrDefault(cart => cart.CartRefId == cartId && cart.ProductRefId == productId);
            if (cart != null)
                return new CartItemsViewModel
                {
                    CartRefId = cart.CartRefId,
                    ProductRefId = cart.ProductRefId,
                    Quantity = cart.Quantity,
                };
            else
                return null;
        }
    }
}
