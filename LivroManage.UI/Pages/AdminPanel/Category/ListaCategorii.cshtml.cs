using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.Category
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ListaCategoriiModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public ListaCategoriiModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager, IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _fileManager = fileManager;
        }

        [BindProperty]
        public IEnumerable<LivroManage.Domain.Models.Category> Categories { get; set; }

        [BindProperty]
        public int Canal { get; set; }
        public async Task<IActionResult> OnGet(int canal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            Canal = canal;
            Categories = new CategoryOperations(_context, _fileManager).GetAll(canal);
            return Page();
        }
    }
}
