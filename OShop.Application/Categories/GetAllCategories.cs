using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OShop.Application.Categories
{
    public class GetAllCategories
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public GetAllCategories(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public IEnumerable<CategoryVMUI> Do() =>
            _context.Categories.AsNoTracking().ToList().Select(categ => new CategoryVMUI
            {
                CategoryId = categ.CategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
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
                Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
                SuperMarketRefId = categ.SuperMarketRefId,
                RestaurantRefId = categ.RestaurantRefId,
            });
        private byte[] getBytes(FileStream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
    }
}
