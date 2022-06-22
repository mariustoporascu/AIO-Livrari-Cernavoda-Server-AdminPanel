using Microsoft.AspNetCore.Http;
using OShop.Application.Companii;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.SubCategories;
using OShop.Database;
using OShop.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class CreateCategory
    {
        private readonly OnlineShopDbContext _context;

        public CreateCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(Category vm)
        {
            _context.Categories.Add(vm);
            await _context.SaveChangesAsync();
            var companie = new GetCompanie(_context).Do(vm.CompanieRefId);
            if (companie.TipCompanieRefId != 2)
            {
                var subCategAuto = new SubCategory
                {
                    Name = vm.Name,
                    CategoryRefId = vm.CategoryId
                };
                _context.SubCategories.Add(subCategAuto);
                await _context.SaveChangesAsync();
            }
        }

    }
}
