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

        public async Task Do(CategoryVMUI vm)
        {
            var categ = new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
                CompanieRefId = vm.CompanieRefId,
            };
            _context.Categories.Add(categ);
            await _context.SaveChangesAsync();
            var companie = new GetCompanie(_context).Do(vm.CompanieRefId);
            if (companie.TipCompanieRefId != 2)
            {
                var subCategAuto = new SubCategory
                {
                    Name = vm.Name,
                    CategoryRefId = categ.CategoryId
                };
                _context.SubCategories.Add(subCategAuto);
                await _context.SaveChangesAsync();
            }
        }

    }
    public class CategoryVMUI
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Image { get; set; }
        public int CompanieRefId { get; set; }
    }
}
