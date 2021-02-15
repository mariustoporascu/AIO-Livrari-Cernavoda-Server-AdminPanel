using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OShop.Application.Products.GetAllProducts;

namespace OShop.Application.Products
{
    public class UpdateProduct
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public UpdateProduct(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(ProductVMUI vm)
        {
            var product = new Product
            {
                ProductId = vm.ProductId,
                Name = vm.Name,
                Description = vm.Description,
                Stock = vm.Stock,
                Price = vm.Price,
                Photo = vm.Photo,
                CategoryRefId = vm.CategoryRefId,
            };
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DoReact(ProductVMReactUI vm)
        {
            var product = _context.Products.FirstOrDefault(prod => prod.ProductId == vm.ProductId);
            product.Name = vm.Name;
            product.Description = vm.Description;
            product.Stock = vm.Stock;
            product.Price = vm.Price;
            product.CategoryRefId = vm.CategoryRefId;
            if (vm.Photo != null)
            {
                if (!string.IsNullOrEmpty(product.Photo))
                {
                    _fileManager.RemoveImage(product.Photo, "ProductPhoto");
                }
                product.Photo = await _fileManager.SaveImage(vm.Photo, "ProductPhoto");
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStockAfterOrder(int productId, int usedQuantity)
        {
            var product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            product.Stock -= usedQuantity;
            await _context.SaveChangesAsync();
        }
    }
}
