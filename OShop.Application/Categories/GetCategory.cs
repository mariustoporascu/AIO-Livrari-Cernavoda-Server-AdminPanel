using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System.Linq;

namespace OShop.Application.Categories
{
    public class GetCategory
    {
        private readonly OnlineShopDbContext _context;

        public GetCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public Category Do(int? categId) => _context.Categories.AsNoTracking().FirstOrDefault(categ => categ.CategoryId == categId);

    }
}
