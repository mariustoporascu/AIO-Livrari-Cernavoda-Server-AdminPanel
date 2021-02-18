using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Collections.Generic;
using System.Linq;

namespace OShop.Application.Products
{
    public class GetAllProducts
    {
        private readonly OnlineShopDbContext _context;

        public GetAllProducts(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductVMUI> Do() =>
            _context.Products.AsNoTracking().ToList().Select(prod => new ProductVMUI
            {
                ProductId = prod.ProductId,
                Name = prod.Name,
                Description = prod.Description,
                Stock = prod.Stock,
                Price = prod.Price,
                Photo = prod.Photo,
                CategoryRefId = prod.CategoryRefId,
            });

    }

}
