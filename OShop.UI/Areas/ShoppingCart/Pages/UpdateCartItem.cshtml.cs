using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.CartItemsA;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Areas.ShoppingCart.Pages
{
    public class UpdateCartItemModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public UpdateCartItemModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ProductId { get; set; }

        [BindProperty]
        public int ShoppingCartId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public int PrevQuantity { get; set; }

        [BindProperty]
        public decimal ProductPrice { get; set; }

        public async Task<IActionResult> OnPost()
        {
            await new UpdateCartItem(_context).Do(new CartItemsViewModel {
                CartRefId = ShoppingCartId,
                ProductRefId = ProductId,
                Quantity = Quantity,
            });
            if(PrevQuantity > Quantity)
                await new UpdateShoppingCart(_context).UpdateTotal(ShoppingCartId, PrevQuantity - Quantity, -ProductPrice);
            else
                await new UpdateShoppingCart(_context).UpdateTotal(ShoppingCartId, Quantity - PrevQuantity, ProductPrice);
            return RedirectToPage("./Index");
        }
    }
}
