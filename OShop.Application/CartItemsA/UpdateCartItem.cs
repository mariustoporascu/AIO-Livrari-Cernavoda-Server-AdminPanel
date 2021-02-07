using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.CartItemsA
{
    public class UpdateCartItem
    {
        private readonly ApplicationDbContext _context;

        public UpdateCartItem(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(CartItemsViewModel vm)
        {
            var cartItems = new CartItems
            {
                CartRefId = vm.CartRefId,
                ProductRefId = vm.ProductRefId,
                Quantity = vm.Quantity,
            };
            _context.CartItems.Update(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
