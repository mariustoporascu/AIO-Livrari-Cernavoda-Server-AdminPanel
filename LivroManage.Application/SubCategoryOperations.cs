using Microsoft.EntityFrameworkCore;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class SubCategoryOperations : EntityOperation<SubCategory>
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;
        public SubCategoryOperations(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public override async Task Create(SubCategory model)
        {
            _context.SubCategories.Add(model);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(int id)
        {
            var subCategory = _context.SubCategories.AsNoTracking().FirstOrDefault(subCateg => subCateg.SubCategoryId == id);
            _context.SubCategories.Remove(subCategory);
            if (!string.IsNullOrEmpty(subCategory.Photo))
                _fileManager.RemoveImage(subCategory.Photo, "SubCategoryPhoto");
            await _context.SaveChangesAsync();
        }

        public override SubCategory Get(int? id)
        {
            return _context.SubCategories.AsNoTracking().FirstOrDefault(subCateg => subCateg.SubCategoryId == id);
        }

        public override IEnumerable<SubCategory> GetAll()
        {
            return _context.SubCategories.AsNoTracking().AsEnumerable();
        }

        public override IEnumerable<SubCategory> GetAll(int categId)
        {
            return _context.SubCategories.AsNoTracking().AsEnumerable().Where(subCateg => subCateg.CategoryRefId == categId);
        }

        public override async Task Update(SubCategory model)
        {
            _context.SubCategories.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
