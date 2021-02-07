using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.CartItemsA
{
    public class GetCartItem
    {
        private readonly ApplicationDbContext _context;

        public GetCartItem(ApplicationDbContext context)
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
