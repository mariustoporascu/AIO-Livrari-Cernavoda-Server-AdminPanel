using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.SubCategories;
using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.SubCategory
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DeleteSubCategoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteSubCategoryModel(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int canal, int categId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            await new DeleteSubCategory(_context, _fileManager).Do(categId);
            return RedirectToPage("./ListaSubCategorii", new { canal = canal });
        }
    }
}
