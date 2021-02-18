using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.Products;
using OShop.Database;
using System.Collections.Generic;

namespace OShop.UI.Areas.AdminPanel.Pages.Product
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
        public IEnumerable<ProductVMUI> Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryViewModel> Categ { get; set; }

        public void OnGet()
        {
            Products = new GetAllProducts(_context).Do();
            Categ = new GetAllCategories(_context).Do();
        }
    }
}
