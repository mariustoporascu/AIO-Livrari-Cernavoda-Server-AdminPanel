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
        private readonly ApplicationDbContext _context;

        public DeleteProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(int productId)
        {
            var product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
