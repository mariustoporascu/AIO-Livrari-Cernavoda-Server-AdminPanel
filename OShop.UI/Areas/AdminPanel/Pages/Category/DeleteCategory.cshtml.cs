using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Areas.AdminPanel.Pages.Category
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeleteCategoryModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public DeleteCategoryModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int categId)
        {
            await new DeleteCategory(_context).Do(categId);
            return RedirectToPage("./Index");
        }
    }
}
