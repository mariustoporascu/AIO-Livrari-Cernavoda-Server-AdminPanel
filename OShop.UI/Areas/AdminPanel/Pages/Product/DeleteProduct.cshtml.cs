using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Products;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Areas.AdminPanel.Pages.Product
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeleteProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int productId)
        {
            await new DeleteProduct(_context).Do(productId);
            return RedirectToPage("./Index");
        }
    }
}
