using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.ShoppingCarts
{
    public class GetShoppingCart
    {
        private readonly OnlineShopDbContext _context;

        public GetShoppingCart(OnlineShopDbContext context)
        {
            _context = context;
        }

        public ShoppingCartViewModel Do(string customerId)
        {
            var shoppingCart = _context.ShoppingCarts.AsNoTracking().FirstOrDefault(cart => cart.CustomerId == customerId && cart.Status == "Active");
            if (shoppingCart == null)
                return null;
            else
                return new ShoppingCartViewModel
                {
                    CartId = shoppingCart.CartId,
                    Status = shoppingCart.Status,
                    TotalInCart = shoppingCart.TotalInCart,
                    Created = shoppingCart.Created,
                    CustomerId = shoppingCart.CustomerId,
                };
        }

        public ShoppingCartViewModel Do(int cartId)
        {
            var shoppingCart = _context.ShoppingCarts.AsNoTracking().FirstOrDefault(cart => cart.CartId == cartId);
            return new ShoppingCartViewModel
            {
                CartId = shoppingCart.CartId,
                Status = shoppingCart.Status,
                TotalInCart = shoppingCart.TotalInCart,
                Created = shoppingCart.Created,
                CustomerId = shoppingCart.CustomerId,
            };
        }
    }
}
