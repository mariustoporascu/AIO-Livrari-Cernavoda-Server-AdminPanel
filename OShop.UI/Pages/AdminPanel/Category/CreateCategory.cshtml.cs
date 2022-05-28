using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Category
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CreateCategoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateCategoryModel(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        [BindProperty]
        public CategoryVMUI Category { get; set; }


        [BindProperty]
        public int Canal { get; set; }
        public async Task<IActionResult> OnGet(int canal, int? categId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.RestaurantRefId)
                return RedirectToPage("/Error");
            if (categId == null)
                Category = new CategoryVMUI();
            else
            {
                Category = new GetCategory(_context).Do(categId);
            }
            Canal = canal;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Category.RestaurantRefId");
            ModelState.Remove("Category.SuperMarketRefId");
            if (ModelState.IsValid)
            {
                Category.RestaurantRefId = Canal;
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
                    await new UpdateCategory(_context).Do(Category);
                }
                else
                    await new CreateCategory(_context).Do(Category);
                return RedirectToPage("./ListaCategorii", new { canal = Canal });
            }
            return RedirectToPage("Error", new { Area = "" });
        }
    }
}
