using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OShop.Application.Categories
{
    public class GetAllCategories
    {
        private readonly OnlineShopDbContext _context;

        public GetAllCategories(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryVMUI> Do() =>
            _context.Categories.AsNoTracking().ToList().Select(categ => new CategoryVMUI
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                SuperMarketRefId = categ.SuperMarketRefId,
                RestaurantRefId = categ.RestaurantRefId,
            });
        public IEnumerable<CategoryVMUI> Do(int canal) =>
            _context.Categories.AsNoTracking()
            .Where(categ => canal == 1 ? categ.SuperMarketRefId != null : categ.RestaurantRefId != null)
            .ToList().Select(categ => new CategoryVMUI
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                SuperMarketRefId = categ.SuperMarketRefId,
                RestaurantRefId = categ.RestaurantRefId,
            });
        public IEnumerable<CategoryVMUI> DoRest(int restaurantId) =>
            _context.Categories.AsNoTracking()
            .Where(categ => categ.RestaurantRefId == restaurantId)
            .Select(categ => new CategoryVMUI
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                RestaurantRefId = categ.RestaurantRefId,
            }).ToList();

    }
}
