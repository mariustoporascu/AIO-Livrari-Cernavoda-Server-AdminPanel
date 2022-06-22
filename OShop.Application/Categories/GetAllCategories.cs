using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OShop.Application.Categories
{
    public class GetAllCategories
    {
        private readonly OnlineShopDbContext _context;

        public GetAllCategories(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> Do() =>
            _context.Categories.AsNoTracking().AsEnumerable();
        public IEnumerable<Category> Do(int canal) =>
            _context.Categories.AsNoTracking().AsEnumerable()
            .Where(categ => categ.CompanieRefId == canal);

    }
}
