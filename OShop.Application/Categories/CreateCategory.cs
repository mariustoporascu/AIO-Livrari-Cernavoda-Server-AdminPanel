using Microsoft.AspNetCore.Http;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using OShop.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class CreateCategory
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateCategory(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(CategoryVMUI vm)
        {
            _context.Categories.Add(new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
            });
            await _context.SaveChangesAsync();
        }

        public async Task DoReact(CategoryVMReactUI vm)
        {
            var category = new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
            };
            if (vm.Photo != null)
            {
                category.Photo = await _fileManager.SaveImage(vm.Photo, "CategoryPhoto");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
    public class CategoryVMReactUI
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile Photo { get; set; }
    }
    public class CategoryVMUI
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
