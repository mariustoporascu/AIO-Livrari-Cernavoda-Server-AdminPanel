using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Category
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DeleteCategoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteCategoryModel(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int canal, int categId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.RestaurantRefId)
                return RedirectToPage("/Error");
            await new DeleteCategory(_context, _fileManager).Do(categId);
            return RedirectToPage("./ListaCategorii", new { canal = canal });
        }
    }
}
