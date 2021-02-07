using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Products
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _context;

        public GetProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductViewModel Do(int? productId)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(prod => prod.ProductId == productId);
            return new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                Photo = product.Photo,
                CategoryRefId = product.CategoryRefId,
            };
        }
    }
}
