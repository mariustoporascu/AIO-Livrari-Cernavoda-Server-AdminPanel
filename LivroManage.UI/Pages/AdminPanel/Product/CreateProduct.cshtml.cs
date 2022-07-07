using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CreateProductModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateProductModel(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }


        [BindProperty]
        public LivroManage.Domain.Models.Product Product { get; set; }

        [BindProperty]
        public IEnumerable<LivroManage.Domain.Models.SubCategory> Categ { get; set; }

        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<MeasuringUnit> UnitatiMasura { get; set; }

        public async Task<IActionResult> OnGet(int canal, int? productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            var canalCateg = new CategoryOperations(_context, _fileManager).GetAll(canal).ToList();
            var canalSubCateg = new List<LivroManage.Domain.Models.SubCategory>();
            foreach (var categ in canalCateg)
            {
                canalSubCateg.AddRange(new SubCategoryOperations(_context, _fileManager).GetAll(categ.CategoryId));
            }
            Categ = canalSubCateg;

            if (productId == null)
                Product = new LivroManage.Domain.Models.Product();
            else
            {
                Product = new ProductOperations(_context, _fileManager).Get(productId);
            }
            UnitatiMasura = _context.MeasuringUnits.AsNoTracking().AsEnumerable();
            Canal = canal;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Product.IsAvailable = true;
                if (Request.Form.Files.Count > 0)
                {
                    var extensionAccepted = new string[] { ".jpg", ".png", ".jpeg" };
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var extension = Path.GetExtension(file.FileName);
                    if (!extensionAccepted.Contains(extension.ToLower()))
                        return RedirectToPage("/Error", new { Area = "" });
                    else
                    {
                        if (!string.IsNullOrEmpty(Product.Photo))
                        {
                            _fileManager.RemoveImage(Product.Photo, "ProductPhoto");
                        }
                        Product.Photo = _fileManager.SaveImage(file, "ProductPhoto");
                    }
                }
                else if (Request.Form.Files.Count == 0)
                {
                    Product.Photo = Product.Photo;
                }


                if (Product.ProductId > 0)
                {
                    await new ProductOperations(_context, _fileManager).Update(Product);
                }
                else
                    await new ProductOperations(_context, _fileManager).Create(Product);
                return RedirectToPage("./ListaProduse", new { canal = Canal });
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
