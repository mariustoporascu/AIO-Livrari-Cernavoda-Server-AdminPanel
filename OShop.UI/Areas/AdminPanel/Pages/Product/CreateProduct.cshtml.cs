using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OShop.Application.Restaurante;
using OShop.Application.SuperMarkets;
using OShop.Domain.Models;
using OShop.Application.UnitatiMasura;
using OShop.Application.SubCategories;

namespace OShop.UI.Areas.AdminPanel.Pages.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CreateProductModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateProductModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [BindProperty]
        public ProductVMUI Product { get; set; }

        [BindProperty]
        public IEnumerable<CategoryVMUI> Categ { get; set; }
        [BindProperty]
        public IEnumerable<SubCategoryVMUI> SubCateg { get; set; }
        [BindProperty]
        public IEnumerable<SuperMarketVMUI> SuperMarkets { get; set; }
        [BindProperty]
        public IEnumerable<RestaurantVMUI> Restaurante { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<UnitateMasuraVMUI> UnitatiMasura { get; set; }

        public void OnGet(int canal, int? productId)
        {
            Categ = new GetAllCategories(_context, _fileManager).Do(canal);
            SuperMarkets = new GetAllSuperMarkets(_context, _fileManager).Do();
            Restaurante = new GetAllRestaurante(_context, _fileManager).Do();
            SubCateg = new GetAllSubCategories(_context, _fileManager).Do();
            if (productId == null)
                Product = new ProductVMUI();
            else
            {
                Product = new GetProduct(_context).Do(productId);
            }
            UnitatiMasura = new GetAllMeasuringUnits(_context).Do();
            Canal = canal;
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Product.RestaurantRefId");
            ModelState.Remove("Product.SuperMarketRefId");
            ModelState.Remove("Product.SubCategoryRefId");
            if (ModelState.IsValid)
            {
                switch (Canal)
                {
                    case 1:
                        Product.RestaurantRefId = null;
                        break;
                    case 2:
                        Product.SubCategoryRefId = null;
                        Product.SuperMarketRefId = null;
                        break;
                    default:
                        Product.SuperMarketRefId = null;
                        Product.RestaurantRefId = null;
                        Product.SubCategoryRefId = null;
                        break;
                }
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
                    await new UpdateProduct(_context, _fileManager).Do(Product);
                }
                else
                    await new CreateProduct(_context, _fileManager).Do(Product);
                return RedirectToPage("./Index", new { canal = Canal });
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
