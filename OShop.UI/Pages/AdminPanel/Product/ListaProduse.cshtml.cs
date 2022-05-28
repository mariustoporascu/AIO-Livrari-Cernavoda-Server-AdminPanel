using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
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
        public IEnumerable<CategoryVMUI> Categ { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<UnitateMasuraVMUI> UnitatiMasura { get; set; }
        public async Task<IActionResult> OnGet(int canal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.RestaurantRefId)
                return RedirectToPage("/Error");
            Canal = canal;
            Products = new GetAllProducts(_context).DoRest(canal);
            Categ = new GetAllCategories(_context).DoRest(canal);
            UnitatiMasura = new GetAllMeasuringUnits(_context).Do();
            return Page();
        }
    }
}
