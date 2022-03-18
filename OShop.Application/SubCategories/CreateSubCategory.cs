using Microsoft.AspNetCore.Http;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using OShop.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OShop.Application.SubCategories
{
    public class CreateSubCategory
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateSubCategory(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(SubCategoryVMUI vm)
        {
            _context.SubCategories.Add(new SubCategory
            {
                SubCategoryId = vm.SubCategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
                CategoryRefId = vm.CategoryRefId,
            });
            await _context.SaveChangesAsync();
        }

        public async Task DoReact(SubCategoryVMReactUI vm)
        {
            var category = new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
            };
            if (vm.Photo != null)
            {
                category.Photo = _fileManager.SaveImage(vm.Photo, "CategoryPhoto");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
    public class SubCategoryVMReactUI
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile Photo { get; set; }
    }
    public class SubCategoryVMUI
    {
        public int SubCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Image { get; set; }
        public int CategoryRefId { get; set; }
    }
}
