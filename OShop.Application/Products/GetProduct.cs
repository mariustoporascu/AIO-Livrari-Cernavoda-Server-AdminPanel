using Microsoft.EntityFrameworkCore;
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
    public class GetProduct
    {
        private readonly OnlineShopDbContext _context;

        public GetProduct(OnlineShopDbContext context)
        {
            _context = context;
        }

        public ProductVMUI Do(int? productId)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(prod => prod.ProductId == productId);
            return new ProductVMUI
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
