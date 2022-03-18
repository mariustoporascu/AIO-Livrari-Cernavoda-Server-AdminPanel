using Microsoft.EntityFrameworkCore;
using OShop.Database;
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

        public SubCategoryVMUI Do(int? categId)
        {
            var categ = _context.SubCategories.AsNoTracking().FirstOrDefault(categ => categ.SubCategoryId == categId);
            return new SubCategoryVMUI
            {
                SubCategoryId = categ.SubCategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                CategoryRefId = categ.CategoryRefId,
            };
        }
    }
}
