﻿using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.Categories
{
    public class GetCategory
    {
        private readonly OnlineShopDbContext _context;

        public GetCategory(OnlineShopDbContext context)
        {
            _context = context;
        }

        public CategoryViewModel Do(int? categId)
        {
            var categ = _context.Categories.AsNoTracking().FirstOrDefault(categ => categ.CategoryId == categId);
            return new CategoryViewModel
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
            };
        }
    }
}
