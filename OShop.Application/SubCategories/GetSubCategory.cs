using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System.Linq;

namespace OShop.Application.SubCategories
{
    public class GetSubCategory
    {
        private readonly OnlineShopDbContext _context;

        public GetSubCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public SubCategory Do(int? categId) => _context.SubCategories.AsNoTracking().FirstOrDefault(categ => categ.SubCategoryId == categId);

    }
}
