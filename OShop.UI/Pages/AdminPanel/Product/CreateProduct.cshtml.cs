using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.SubCategories;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Product
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
        public ProductVMUI Product { get; set; }

        [BindProperty]
        public IEnumerable<SubCategoryVMUI> Categ { get; set; }

        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<UnitateMasuraVMUI> UnitatiMasura { get; set; }

        public async Task<IActionResult> OnGet(int canal, int? productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            var canalCateg = new GetAllCategories(_context).Do(canal).ToList();
            var canalSubCateg = new List<SubCategoryVMUI>();
            foreach (var categ in canalCateg)
            {
                canalSubCateg.AddRange(new GetAllSubCategories(_context).Do(categ.CategoryId));
            }
            Categ = canalSubCateg;

            if (productId == null)
                Product = new ProductVMUI();
            else
            {
                Product = new GetProduct(_context).Do(productId);
            }
            UnitatiMasura = new GetAllMeasuringUnits(_context).Do();
            Canal = canal;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
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
                    await new UpdateProduct(_context).Do(Product);
                }
                else
                    await new CreateProduct(_context).Do(Product);
                return RedirectToPage("./ListaProduse", new { canal = Canal });
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
