using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Categories
{
    public class GetAllCategories
    {
        private readonly ApplicationDbContext _context;

        public GetAllCategories(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryViewModel> Do() =>
            _context.Categories.AsNoTracking().ToList().Select(categ => new CategoryViewModel
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
            });
    }
}
