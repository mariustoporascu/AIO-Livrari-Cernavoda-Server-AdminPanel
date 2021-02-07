using OShop.Application.ShoppingCarts;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.CartItemsA
{
    public class CreateCartItem
    {
        private readonly ApplicationDbContext _context;

        public CreateCartItem(ApplicationDbContext context)
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
