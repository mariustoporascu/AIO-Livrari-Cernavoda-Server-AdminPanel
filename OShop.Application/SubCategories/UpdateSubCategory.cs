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

        public UpdateSubCategory(OnlineShopDbContext context)
        {
            _context = context;
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

    }
}
