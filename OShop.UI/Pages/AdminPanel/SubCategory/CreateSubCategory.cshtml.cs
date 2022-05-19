using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.SubCategories;
using OShop.Database;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.SubCategory
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateSubCategoryModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateSubCategoryModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [BindProperty]
        public SubCategoryVMUI SubCategory { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public int Category { get; set; }
        public void OnGet(int canal, int category, int? subcategId)
        {
            //if (subcategId == null)
            //    SubCategory = new SubCategoryVMUI { CategoryRefId = category };
            //else
            //{
            //    SubCategory = new GetSubCategory(_context).Do(subcategId);
            //}
            //Canal = canal;
            //Category = category;
        }

        public async Task<IActionResult> OnPost()
        {
            //if (ModelState.IsValid)
            //{
            //    if (Request.Form.Files.Count > 0)
            //    {
            //        var extensionAccepted = new string[] { ".jpg", ".png", ".jpeg" };
            //        IFormFile file = Request.Form.Files.FirstOrDefault();
            //        var extension = Path.GetExtension(file.FileName);
            //        if (!extensionAccepted.Contains(extension.ToLower()))
            //            return RedirectToPage("/Error", new { Area = "" });
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(SubCategory.Photo))
            //            {
            //                _fileManager.RemoveImage(SubCategory.Photo, "SubCategoryPhoto");
            //            }
            //            SubCategory.Photo = _fileManager.SaveImage(file, "SubCategoryPhoto");
            //        }

            //    }
            //    else if (Request.Form.Files.Count == 0)
            //    {
            //        SubCategory.Photo = SubCategory.Photo;
            //    }
            //    if (SubCategory.SubCategoryId > 0)
            //    {
            //        await new UpdateSubCategory(_context, _fileManager).Do(SubCategory);
            //    }
            //    else
            //        await new CreateSubCategory(_context, _fileManager).Do(SubCategory);
            //    return RedirectToPage("./Index", new { canal = Canal, category = Category });
            //}
            return RedirectToPage("Error", new { Area = "" });
        }
    }
}
