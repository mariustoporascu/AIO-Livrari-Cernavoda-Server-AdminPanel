using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DeleteProductModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteProductModel(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int canal, int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            await new ProductOperations(_context, _fileManager).Delete(productId);
            return RedirectToPage("./ListaProduse", new { canal = canal });
        }
    }
}
