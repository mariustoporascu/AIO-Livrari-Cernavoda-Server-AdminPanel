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

        public async Task Do(SubCategory vm)
        {
            _context.SubCategories.Add(vm);
            await _context.SaveChangesAsync();
        }


    }


}
