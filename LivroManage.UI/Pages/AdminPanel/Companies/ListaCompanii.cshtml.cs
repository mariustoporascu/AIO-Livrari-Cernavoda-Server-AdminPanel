using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using System.Collections.Generic;

namespace LivroManage.UI.Pages.AdminPanel.Companies
{
    [Authorize(Roles = "SuperAdmin")]
    public class ListaCompaniiModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public ListaCompaniiModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }


        [BindProperty]
        public IEnumerable<CompanieVM> Companii { get; set; }
        public IActionResult OnGet()
        {

            Companii = new CompanieOperations(_context, _fileManager).GetAllVM();

            return Page();
        }
    }
}
