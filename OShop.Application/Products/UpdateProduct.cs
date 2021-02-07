using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Products
{
    public class UpdateProduct
    {
        private readonly ApplicationDbContext _context;

        public UpdateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(ProductViewModel vm)
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
        public async Task UpdateStockAfterOrder(int productId, int usedQuantity)
        {
            var product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            product.Stock -= usedQuantity;
            await _context.SaveChangesAsync();
        }
    }
}
