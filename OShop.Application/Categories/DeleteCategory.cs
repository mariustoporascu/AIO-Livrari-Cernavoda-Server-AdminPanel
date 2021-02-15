using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class DeleteCategory
    {
        private readonly OnlineShopDbContext _context;

        public DeleteCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(int categId)
        {
            var categ = _context.Categories.FirstOrDefault(categ => categ.CategoryId == categId);
            _context.Categories.Remove(categ);
            await _context.SaveChangesAsync();
        }
    }
}
