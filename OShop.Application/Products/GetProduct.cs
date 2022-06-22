using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
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

        public Product Do(int? productId) => _context.Products.AsNoTracking().FirstOrDefault(prod => prod.ProductId == productId);

    }
}
