using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.CartItemsA;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Areas.ShoppingCart.Pages
{
    public class RemoveCartItemModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public RemoveCartItemModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int ShoppingCartId, int ProductId, int Quantity, decimal ProductPrice)
        {
            await new DeleteCartItem(_context).Do(ShoppingCartId, ProductId);
            await new UpdateShoppingCart(_context).UpdateTotal(ShoppingCartId, Quantity, -ProductPrice);
            return RedirectToPage("./Index");
        }
    }
}
