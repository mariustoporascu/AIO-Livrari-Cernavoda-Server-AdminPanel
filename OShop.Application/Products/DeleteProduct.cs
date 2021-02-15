using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Products
{
    public class DeleteProduct
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteProduct(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(int productId)
        {
            var product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            _context.Products.Remove(product);
            if(!string.IsNullOrEmpty(product.Photo))
                _fileManager.RemoveImage(product.Photo, "ProductPhoto");
            await _context.SaveChangesAsync();
        }
    }
}
