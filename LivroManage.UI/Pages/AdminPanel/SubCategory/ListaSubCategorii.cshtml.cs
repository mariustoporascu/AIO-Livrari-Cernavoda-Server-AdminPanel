using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.SubCategory
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ListaSubCategoriiModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileManager _fileManager;
        private readonly OnlineShopDbContext _context;

        public ListaSubCategoriiModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager, IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _fileManager = fileManager;
        }

        [BindProperty]
        public IEnumerable<LivroManage.Domain.Models.SubCategory> SubCategories { get; set; }
        [BindProperty]
        public IEnumerable<LivroManage.Domain.Models.Category> Categories { get; set; }
        public int Canal { get; set; }
        public async Task<IActionResult> OnGet(int canal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            Canal = canal;
            var canalCateg = new CategoryOperations(_context, _fileManager).GetAll(canal).ToList();
            Categories = canalCateg;
            var canalSubCateg = new List<LivroManage.Domain.Models.SubCategory>();
            foreach (var categ in canalCateg)
            {
                canalSubCateg.AddRange(new SubCategoryOperations(_context, _fileManager).GetAll(categ.CategoryId));
            }
            SubCategories = canalSubCateg;

            return Page();
        }
    }
}
