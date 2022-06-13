using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.SubCategories;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.SubCategory
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ListaSubCategoriiModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;

        public ListaSubCategoriiModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<SubCategoryVMUI> SubCategories { get; set; }
        public int Canal { get; set; }
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
            SubCategories = canalSubCateg;

            return Page();
        }
    }
}
