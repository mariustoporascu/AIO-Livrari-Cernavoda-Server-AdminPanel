using Microsoft.AspNetCore.Http;
using OShop.Application.FileManager;
using OShop.Application.Products;
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
            _context.Categories.Add(new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
                SuperMarketRefId = vm.SuperMarketRefId,
                RestaurantRefId = vm.RestaurantRefId,
            });
            await _context.SaveChangesAsync();
        }

    }
    public class CategoryVMUI
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Image { get; set; }
        public int? SuperMarketRefId { get; set; }
        public int? RestaurantRefId { get; set; }
    }
}
