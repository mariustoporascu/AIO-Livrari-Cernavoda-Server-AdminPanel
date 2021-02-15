using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Areas.AdminPanel.Pages.Category
{
    [Authorize(Roles = "SuperAdmin")]
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
        public CategoryViewModel Category { get; set; }

        public void OnGet(int? categId)
        {
            if (categId == null)
                Category = new CategoryViewModel();
            else
            {
                var getCategory = new GetCategory(_context).Do(categId);
                Category = new CategoryViewModel
                {
                    CategoryId = getCategory.CategoryId,
                    Name = getCategory.Name,
                    Photo = getCategory.Photo,
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
                    if (!string.IsNullOrEmpty(Category.Photo))
                    {
                        _fileManager.RemoveImage(Category.Photo, "CategoryPhoto");
                    }
                    Category.Photo = await _fileManager.SaveImage(file, "CategoryPhoto");
                }
                else if (Request.Form.Files.Count == 0)
                {
                    Category.Photo = Category.Photo;
                }
                if (Category.CategoryId > 0)
                {
                    var category = new CategoryViewModel
                    {
                        CategoryId = Category.CategoryId,
                        Name = Category.Name,
                        Photo = Category.Photo,
                    };
                    await new UpdateCategory(_context).Do(category);
                }
                else
                    await new CreateCategory(_context).Do(Category);
                return RedirectToPage("./Index");
            }
            return RedirectToPage("Error");
        }
    }
}
