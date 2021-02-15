using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OShop.Application.Products.GetAllProducts;

namespace OShop.UI.Areas.AdminPanel.Pages.Product
{
    [Authorize(Roles = "SuperAdmin")]
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
        public IEnumerable<CategoryViewModel> Categ { get; set; }

        public void OnGet(int? productId)
        {
            Categ = new GetAllCategories(_context).Do();
            if (productId == null)
                Product = new ProductVMUI();
            else
            {
                var getProduct = new GetProduct(_context).Do(productId);
                Product = new ProductVMUI
                {
                    ProductId = getProduct.ProductId,
                    Name = getProduct.Name,
                    Description = getProduct.Description,
                    Stock = getProduct.Stock,
                    Price = getProduct.Price,
                    Photo = getProduct.Photo,
                    CategoryRefId = getProduct.CategoryRefId,
                };
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    if (!string.IsNullOrEmpty(Product.Photo))
                    {
                        _fileManager.RemoveImage(Product.Photo, "ProductPhoto");
                    }
                    Product.Photo = await _fileManager.SaveImage(file, "ProductPhoto");
                }
                else if (Request.Form.Files.Count == 0)
                {
                    Product.Photo = Product.Photo;
                }


                if (Product.ProductId > 0)
                {
                    var product = new ProductVMUI
                    {
                        ProductId = Product.ProductId,
                        Name = Product.Name,
                        Description = Product.Description,
                        Stock = Product.Stock,
                        Price = Product.Price,
                        Photo = Product.Photo,
                        CategoryRefId = Product.CategoryRefId,
                    };
                    await new UpdateProduct(_context, _fileManager).Do(product);
                }
                else
                    await new CreateProduct(_context, _fileManager).Do(Product);
                return RedirectToPage("./Index");
            }
            return RedirectToPage("Error");
        }
    }
}
