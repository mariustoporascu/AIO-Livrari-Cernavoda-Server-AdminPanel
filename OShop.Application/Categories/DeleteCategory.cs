using OShop.Application.FileManager;
using OShop.Database;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class DeleteCategory
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteCategory(OnlineShopDbContext context,IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(int categoryId)
        {
            var category = _context.Categories
                .FirstOrDefault(categ => categ.CategoryId == categoryId);
            _context.Categories.Remove(category);
            if (!string.IsNullOrEmpty(category.Photo))
                _fileManager.RemoveImage(category.Photo, "CategoryPhoto");
            await _context.SaveChangesAsync();
        }
    }
}
