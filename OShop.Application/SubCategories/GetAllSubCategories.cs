using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
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

        public IEnumerable<SubCategoryVMUI> Do() =>
            _context.SubCategories.AsNoTracking().ToList().Select(categ => new SubCategoryVMUI
            {
                SubCategoryId = categ.SubCategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                CategoryRefId = categ.CategoryRefId,
            });
        public IEnumerable<SubCategoryVMUI> Do(int categoryId) =>
            _context.SubCategories.AsNoTracking()
            .Where(categ => categ.CategoryRefId == categoryId)
            .ToList().Select(categ => new SubCategoryVMUI
            {
                SubCategoryId = categ.SubCategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                CategoryRefId = categ.CategoryRefId,
            });
    }
}
