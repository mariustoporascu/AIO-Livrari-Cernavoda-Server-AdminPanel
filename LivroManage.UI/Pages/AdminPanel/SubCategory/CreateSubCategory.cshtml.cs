using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.SubCategory
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CreateSubCategoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateSubCategoryModel(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LivroManage.Domain.Models.SubCategory SubCategory { get; set; }
        [BindProperty]
        public IEnumerable<LivroManage.Domain.Models.Category> Categ { get; set; }

        [BindProperty]
        public int Canal { get; set; }
        public async Task<IActionResult> OnGet(int canal, int? categId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (canal != user.CompanieRefId)
                return RedirectToPage("/Error");
            Categ = new CategoryOperations(_context, _fileManager).GetAll(canal).ToList();
            if (categId == null)
                SubCategory = new LivroManage.Domain.Models.SubCategory();
            else
            {
                SubCategory = new SubCategoryOperations(_context, _fileManager).Get(categId);
            }
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
                        if (!string.IsNullOrEmpty(SubCategory.Photo))
                        {
                            _fileManager.RemoveImage(SubCategory.Photo, "SubCategoryPhoto");
                        }
                        SubCategory.Photo = _fileManager.SaveImage(file, "SubCategoryPhoto");
                    }

                }
                else if (Request.Form.Files.Count == 0)
                {
                    SubCategory.Photo = SubCategory.Photo;
                }
                if (SubCategory.SubCategoryId > 0)
                {
                    await new SubCategoryOperations(_context, _fileManager).Update(SubCategory);
                }
                else
                    await new SubCategoryOperations(_context, _fileManager).Create(SubCategory);
                return RedirectToPage("./ListaSubCategorii", new { canal = Canal });
            }
            return RedirectToPage("Error", new { Area = "" });
        }
    }
}
