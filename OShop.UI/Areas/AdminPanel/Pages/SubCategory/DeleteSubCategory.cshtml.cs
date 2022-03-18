using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.SubCategories;
using OShop.Application.FileManager;
using OShop.Database;
using System.Threading.Tasks;

namespace OShop.UI.Areas.AdminPanel.Pages.SubCategory
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DeleteSubCategoryModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteSubCategoryModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> OnGet(int canal, int category, int subcategId)
        {
            await new DeleteSubCategory(_context, _fileManager).Do(subcategId);
            return RedirectToPage("./Index", new { canal = canal, category = category });
        }
    }
}
