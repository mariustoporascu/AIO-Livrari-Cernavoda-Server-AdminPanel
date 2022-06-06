using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Product
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
            await new DeleteProduct(_context, _fileManager).Do(productId);
            return RedirectToPage("./ListaProduse", new { canal = canal });
        }
    }
}
