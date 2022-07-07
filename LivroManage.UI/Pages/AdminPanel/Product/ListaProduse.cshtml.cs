using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ListaProduseModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileManager _fileManager;
        private readonly OnlineShopDbContext _context;

        public ListaProduseModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager, IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _fileManager = fileManager;
        }


        [BindProperty]
        public IEnumerable<ProductVM> Products { get; set; }
        [BindProperty]
        public IEnumerable<LivroManage.Domain.Models.SubCategory> Categ { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<MeasuringUnit> UnitatiMasura { get; set; }
        public async Task<IActionResult> OnGet(int canal)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            Canal = canal;
            var canalCateg = new CategoryOperations(_context, _fileManager).GetAll(canal).ToList();
            var canalSubCateg = new List<LivroManage.Domain.Models.SubCategory>();
            foreach (var categ in canalCateg)
            {
                canalSubCateg.AddRange(new SubCategoryOperations(_context, _fileManager).GetAll(categ.CategoryId));
            }
            Categ = canalSubCateg;
            var canalProducts = new List<ProductVM>();
            foreach (var subcateg in canalSubCateg)
            {
                canalProducts.AddRange(new ProductOperations(_context, _fileManager).GetAllVMSub(subcateg.SubCategoryId));
            }
            Products = canalProducts;
            UnitatiMasura = _context.MeasuringUnits.AsNoTracking().AsEnumerable();
            return Page();
        }
    }
}
