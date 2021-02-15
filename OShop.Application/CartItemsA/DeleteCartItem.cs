using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.CartItemsA
{
    public class DeleteCartItem
    {
        private readonly OnlineShopDbContext _context;

        public DeleteCartItem(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(int cartId, int productId)
        {
            var cartItem = new CartItems
            {
                CartRefId = cartId,
                ProductRefId = productId,
            };
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
