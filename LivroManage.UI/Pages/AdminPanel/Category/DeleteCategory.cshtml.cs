using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.Category
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
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            await new CategoryOperations(_context, _fileManager).Delete(categId);
            return RedirectToPage("./ListaCategorii", new { canal = canal });
        }
    }
}
