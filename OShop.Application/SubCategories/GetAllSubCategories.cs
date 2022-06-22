using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OShop.Application.SubCategories
{
    public class GetAllSubCategories
    {
        private readonly OnlineShopDbContext _context;

        public GetAllSubCategories(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SubCategory> Do() =>
            _context.SubCategories.AsNoTracking().AsEnumerable();
        public IEnumerable<SubCategory> Do(int categoryId) =>
            _context.SubCategories.AsNoTracking().AsEnumerable()
            .Where(categ => categ.CategoryRefId == categoryId)
            .ToList();
    }
}
