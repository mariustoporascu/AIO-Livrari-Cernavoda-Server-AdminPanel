using Microsoft.EntityFrameworkCore;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class CategoryOperations : EntityOperation<Category>
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;
        public CategoryOperations(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public override async Task Create(Category model)
        {
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            var companie = new CompanieOperations(_context, _fileManager).Get(model.CompanieRefId);
            if (companie.TipCompanieRefId != 2)
            {
                var subCategAuto = new SubCategory
                {
                    Name = model.Name,
                    CategoryRefId = model.CategoryId
                };
                _context.SubCategories.Add(subCategAuto);
                await _context.SaveChangesAsync();
            }
        }

        public override async Task Delete(int id)
        {
            var category = _context.Categories.AsNoTracking().FirstOrDefault(categ => categ.CategoryId == id);
            _context.Categories.Remove(category);
            if (!string.IsNullOrEmpty(category.Photo))
                _fileManager.RemoveImage(category.Photo, "CategoryPhoto");
            await _context.SaveChangesAsync();
        }

        public override Category Get(int? id)
        {
            return _context.Categories.AsNoTracking().FirstOrDefault(categ => categ.CategoryId == id);
        }

        public override IEnumerable<Category> GetAll()
        {
            return _context.Categories.AsNoTracking().AsEnumerable();
        }

        public override IEnumerable<Category> GetAll(int canal)
        {
            return _context.Categories.AsNoTracking().AsEnumerable().Where(categ => categ.CompanieRefId == canal);
        }

        public override async Task Update(Category model)
        {
            _context.Categories.Update(model);
            await _context.SaveChangesAsync();
            var companie = new CompanieOperations(_context, _fileManager).Get(model.CompanieRefId);
            if (companie.TipCompanieRefId != 2)
            {
                var subCategAuto = _context.SubCategories.AsNoTracking().FirstOrDefault(sub => sub.CategoryRefId == model.CategoryId);
                if (subCategAuto != null)
                {
                    subCategAuto.Name = model.Name;
                    _context.SubCategories.Update(subCategAuto);
                }
                else
                {
                    subCategAuto = new SubCategory
                    {
                        Name = model.Name,
                        CategoryRefId = model.CategoryId,
                    };
                    _context.SubCategories.Add(subCategAuto);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
