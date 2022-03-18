using OShop.Application.FileManager;
using OShop.Database;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.SubCategories
{
    public class DeleteSubCategory
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteSubCategory(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(int categoryId)
        {
            var category = _context.SubCategories
                .FirstOrDefault(categ => categ.SubCategoryId == categoryId);
            _context.SubCategories.Remove(category);
            if (!string.IsNullOrEmpty(category.Photo))
                _fileManager.RemoveImage(category.Photo, "SubCategoryPhoto");
            await _context.SaveChangesAsync();
        }
    }
}
