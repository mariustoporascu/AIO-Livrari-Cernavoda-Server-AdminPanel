using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.Products
{
    public class GetProduct
    {
        private readonly OnlineShopDbContext _context;

        public GetProduct(OnlineShopDbContext context)
        {
            _context = context;
        }

        public ProductVMUI Do(string productName)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(prod => prod.Name.ToLower() == productName.ToLower());
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
