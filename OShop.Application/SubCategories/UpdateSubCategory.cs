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

        public async Task Do(SubCategory vm)
        {

            _context.SubCategories.Update(vm);
            await _context.SaveChangesAsync();
        }

    }
}
