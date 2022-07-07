using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.SubCategory
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
            await new SubCategoryOperations(_context, _fileManager).Delete(categId);
            return RedirectToPage("./ListaSubCategorii", new { canal = canal });
        }
    }
}
