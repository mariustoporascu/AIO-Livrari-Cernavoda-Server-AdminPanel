using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
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

        public IEnumerable<CategoryVMUI> Do() =>
            _context.Categories.AsNoTracking().AsEnumerable().Select(categ => new CategoryVMUI
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                CompanieRefId = categ.CompanieRefId,
            });
        public IEnumerable<CategoryVMUI> Do(int canal) =>
            _context.Categories.AsNoTracking().AsEnumerable()
            .Where(categ => categ.CompanieRefId == canal)
            .Select(categ => new CategoryVMUI
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                CompanieRefId = categ.CompanieRefId,
            });

    }
}
