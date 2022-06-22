using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Companii;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Category
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ListaCategoriiModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;

        public ListaCategoriiModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<OShop.Domain.Models.Category> Categories { get; set; }

        [BindProperty]
        public int Canal { get; set; }
        public async Task<IActionResult> OnGet(int canal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            Canal = canal;
            Categories = new GetAllCategories(_context).Do(canal);
            return Page();
        }
    }
}
