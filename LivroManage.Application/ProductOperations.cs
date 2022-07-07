using Microsoft.EntityFrameworkCore;
using LivroManage.Application.FileManager;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class ProductOperations : EntityOperation<Product>
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;
        public ProductOperations(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public override async Task Create(Product model)
        {
            _context.Products.Add(model);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(int id)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(prod => prod.ProductId == id);
            _context.Products.Remove(product);
            if (!string.IsNullOrEmpty(product.Photo))
                _fileManager.RemoveImage(product.Photo, "ProductPhoto");
            await _context.SaveChangesAsync();
        }

        public override Product Get(int? id)
        {
            return _context.Products.AsNoTracking().FirstOrDefault(prod => prod.ProductId == id);
        }

        public override IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Product> GetAll(int canal)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ProductVM> GetAllVM()
        {

            return _context.Products.AsNoTracking().AsEnumerable()
                .Select(prod => new ProductVM(prod)
                {
                    ExtraProducts = _context.ExtraProduse.AsNoTracking().AsEnumerable().Where(extr => extr.ProductRefId == prod.ProductId),
                });
        }
        public IEnumerable<ProductVM> GetAllVMSub(int subCategId)
        {

            return _context.Products.AsNoTracking().AsEnumerable().Where(prod => prod.SubCategoryRefId == subCategId)
                .Select(prod => new ProductVM(prod)
                {
                    ExtraProducts = _context.ExtraProduse.AsNoTracking().AsEnumerable().Where(extr => extr.ProductRefId == prod.ProductId),
                });
        }

        public override async Task Update(Product model)
        {
            _context.Products.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
