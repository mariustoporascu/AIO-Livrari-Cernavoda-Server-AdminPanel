using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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

        public async Task Do(CategoryViewModel vm)
        {
            var category = new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
            };
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
