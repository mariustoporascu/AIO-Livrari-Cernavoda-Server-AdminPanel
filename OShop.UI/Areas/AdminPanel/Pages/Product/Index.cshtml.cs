using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IEnumerable<ProductViewModel> Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryViewModel> Categ { get; set; }

        public void OnGet()
        {
            Products = new GetAllProducts(_context).Do();
            Categ = new GetAllCategories(_context).Do();
        }
    }
}
