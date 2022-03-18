using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Restaurante;
using OShop.Application.SuperMarkets;
using OShop.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Areas.AdminPanel.Pages.Category
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CreateCategoryModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateCategoryModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [BindProperty]
        public CategoryVMUI Category { get; set; }
        [BindProperty]
        public IEnumerable<SuperMarketVMUI> SuperMarkets { get; set; }
        [BindProperty]
        public IEnumerable<RestaurantVMUI> Restaurante { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        public void OnGet(int canal, int? categId)
        {
            SuperMarkets = new GetAllSuperMarkets(_context, _fileManager).Do();
            Restaurante = new GetAllRestaurante(_context, _fileManager).Do();
            if (categId == null)
                Category = new CategoryVMUI();
            else
            {
                Category = new GetCategory(_context).Do(categId);
            }
            Canal = canal;
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Product.RestaurantRefId");
            ModelState.Remove("Product.SuperMarketRefId");
            if (ModelState.IsValid)
            {
                switch (Canal)
                {
                    case 1:
                        Category.RestaurantRefId = null;
                        break;
                    case 2:
                        Category.SuperMarketRefId = null;
                        break;
                    default:
                        Category.SuperMarketRefId = null;
                        Category.RestaurantRefId = null;
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
                        if (!string.IsNullOrEmpty(Category.Photo))
                        {
                            _fileManager.RemoveImage(Category.Photo, "CategoryPhoto");
                        }
                        Category.Photo = _fileManager.SaveImage(file, "CategoryPhoto");
                    }

                }
                else if (Request.Form.Files.Count == 0)
                {
                    Category.Photo = Category.Photo;
                }
                if (Category.CategoryId > 0)
                {
                    await new UpdateCategory(_context, _fileManager).Do(Category);
                }
                else
                    await new CreateCategory(_context, _fileManager).Do(Category);
                return RedirectToPage("./Index", new { canal = Canal });
            }
            return RedirectToPage("Error", new { Area = "" });
        }
    }
}
