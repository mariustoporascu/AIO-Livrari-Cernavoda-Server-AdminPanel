using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.CartItemsA
{
    public class GetCartItems
    {
        private readonly OnlineShopDbContext _context;

        public GetCartItems(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CartItemsViewModel> Do(int cartId) =>
            _context.CartItems.AsNoTracking().Where(cart => cart.CartRefId == cartId)
                .Select(cartItem => new CartItemsViewModel {
                    CartRefId = cartItem.CartRefId,
                    ProductRefId = cartItem.ProductRefId,
                    Quantity = cartItem.Quantity,
                });
    }
}
