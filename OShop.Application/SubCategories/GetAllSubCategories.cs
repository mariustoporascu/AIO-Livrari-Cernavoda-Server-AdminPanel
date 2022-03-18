using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OShop.Application.SubCategories
{
    public class GetAllSubCategories
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public GetAllSubCategories(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public IEnumerable<SubCategoryVMUI> Do() =>
            _context.SubCategories.AsNoTracking().ToList().Select(categ => new SubCategoryVMUI
            {
                SubCategoryId = categ.SubCategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
                CategoryRefId = categ.CategoryRefId,
            });
        public IEnumerable<SubCategoryVMUI> Do(int categoryId) =>
            _context.SubCategories.AsNoTracking()
            .Where(categ => categ.CategoryRefId == categoryId)
            .ToList().Select(categ => new SubCategoryVMUI
            {
                SubCategoryId = categ.SubCategoryId,
                Name = categ.Name,
                Photo = categ.Photo,
                Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
                CategoryRefId = categ.CategoryRefId,
            });
        private byte[] getBytes(FileStream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
    }
}
