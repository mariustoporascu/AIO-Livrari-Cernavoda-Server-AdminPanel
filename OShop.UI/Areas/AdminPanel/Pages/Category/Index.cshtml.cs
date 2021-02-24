using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Database;
using System.Collections.Generic;

namespace OShop.UI.Areas.AdminPanel.Pages.Category
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public IndexModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IEnumerable<CategoryVMUI> Categories { get; set; }

        public void OnGet()
        {
            Categories = new GetAllCategories(_context).Do();
        }
    }
}
