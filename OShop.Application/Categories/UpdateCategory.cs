using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class UpdateCategory
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public UpdateCategory(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(CategoryVMUI vm)
        {
            var category = new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
            };
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DoReact(CategoryVMReactUI vm)
        {
            var category = _context.Categories.FirstOrDefault(categ => categ.CategoryId == vm.CategoryId);
            category.Name = vm.Name;
            if (vm.Photo != null)
            {
                if (!string.IsNullOrEmpty(category.Photo))
                {
                    _fileManager.RemoveImage(category.Photo, "CategoryPhoto");
                }
                category.Photo = await _fileManager.SaveImage(vm.Photo, "CategoryPhoto");
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
