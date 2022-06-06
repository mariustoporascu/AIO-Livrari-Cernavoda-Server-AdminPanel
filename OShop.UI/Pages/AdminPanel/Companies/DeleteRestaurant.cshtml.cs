using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.Companii;
using OShop.Database;
using OShop.Domain.Models;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Companies
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeleteRestaurantModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteRestaurantModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> OnGet(int restId)
        {
            await new DeleteCompanie(_context, _fileManager).Do(restId);
            return RedirectToPage("./ListaRestaurante");
        }
    }
}
