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
    public class CreateCategory
    {
        private readonly OnlineShopDbContext _context;

        public CreateCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(CategoryViewModel vm)
        {
            _context.Categories.Add(new Category
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Photo = vm.Photo,
            });
            await _context.SaveChangesAsync();
        }
    }
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
