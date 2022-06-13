using Microsoft.EntityFrameworkCore;
using OShop.Application.Companii;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class UpdateCategory
    {
        private readonly OnlineShopDbContext _context;

        public UpdateCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(CategoryVMUI vm)
        {
            var category = new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
                CompanieRefId = vm.CompanieRefId,
            };
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            var companie = new GetCompanie(_context).Do(vm.CompanieRefId);
            if (companie.TipCompanieRefId != 2)
            {
                var subCategAuto = _context.SubCategories.AsNoTracking().FirstOrDefault(sub => sub.CategoryRefId == vm.CategoryId);
                if (subCategAuto != null)
                {
                    subCategAuto.Name = vm.Name;
                    _context.SubCategories.Update(subCategAuto);
                }
                else
                {
                    subCategAuto = new SubCategory
                    {
                        Name = vm.Name,
                        CategoryRefId = category.CategoryId,
                    };
                    _context.SubCategories.Add(subCategAuto);
                }
                await _context.SaveChangesAsync();
            }
        }


    }
}
