using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.SubCategories;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ListaProduseModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;

        public ListaProduseModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }
        [BindProperty]
        public IEnumerable<SubCategoryVMUI> Categ { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<UnitateMasuraVMUI> UnitatiMasura { get; set; }
        public async Task<IActionResult> OnGet(int canal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            Canal = canal;
            var canalCateg = new GetAllCategories(_context).Do(canal).ToList();
            var canalSubCateg = new List<SubCategoryVMUI>();
            foreach (var categ in canalCateg)
            {
                canalSubCateg.AddRange(new GetAllSubCategories(_context).Do(categ.CategoryId));
            }
            Categ = canalSubCateg;
            var canalProducts = new List<ProductVMUI>();
            foreach (var subcateg in canalSubCateg)
            {
                canalProducts.AddRange(new GetAllProducts(_context).Do(subcateg.SubCategoryId));
            }
            Products = canalProducts;
            UnitatiMasura = new GetAllMeasuringUnits(_context).Do();
            return Page();
        }
    }
}
