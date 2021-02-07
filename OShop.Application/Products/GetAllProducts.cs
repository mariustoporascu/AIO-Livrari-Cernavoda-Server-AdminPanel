using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Products
{
    public class GetAllProducts
    {
        private readonly ApplicationDbContext _context;

        public GetAllProducts(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _context.Products.AsNoTracking().ToList().Select(prod => new ProductViewModel
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
