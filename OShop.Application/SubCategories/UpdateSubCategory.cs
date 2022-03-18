using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.SubCategories
{
    public class UpdateSubCategory
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public UpdateSubCategory(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(SubCategoryVMUI vm)
        {
            var category = new SubCategory
            {
                SubCategoryId = vm.SubCategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
                CategoryRefId = vm.CategoryRefId,
            };
            _context.SubCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DoReact(SubCategoryVMReactUI vm)
        {
            var category = _context.Categories.FirstOrDefault(categ => categ.CategoryId == vm.CategoryId);
            category.Name = vm.Name;
            if (vm.Photo != null)
            {
                if (!string.IsNullOrEmpty(category.Photo))
                {
                    _fileManager.RemoveImage(category.Photo, "CategoryPhoto");
                }
                category.Photo = _fileManager.SaveImage(vm.Photo, "CategoryPhoto");
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
