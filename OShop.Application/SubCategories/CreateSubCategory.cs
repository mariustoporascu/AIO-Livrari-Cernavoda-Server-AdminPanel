using Microsoft.AspNetCore.Http;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using OShop.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OShop.Application.SubCategories
{
    public class CreateSubCategory
    {
        private readonly OnlineShopDbContext _context;

        public CreateSubCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(SubCategoryVMUI vm)
        {
            _context.SubCategories.Add(new SubCategory
            {
                SubCategoryId = vm.SubCategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
                CategoryRefId = vm.CategoryRefId,
            });
            await _context.SaveChangesAsync();
        }


    }

    public class SubCategoryVMUI
    {
        public int SubCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Image { get; set; }
        public int CategoryRefId { get; set; }
    }
}
