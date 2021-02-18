using OShop.Database;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class DeleteCategory
    {
        private readonly OnlineShopDbContext _context;

        public DeleteCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(string categName)
        {
            var categ = _context.Categories.FirstOrDefault(categ => categ.Name == categName);
            _context.Categories.Remove(categ);
            await _context.SaveChangesAsync();
        }
    }
}
