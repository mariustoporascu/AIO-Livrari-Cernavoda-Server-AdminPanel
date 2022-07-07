using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.Companies
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeleteCompanieModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteCompanieModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> OnGet(int restId)
        {
            await new CompanieOperations(_context, _fileManager).Delete(restId);
            return RedirectToPage("./ListaCompanii");
        }
    }
}
