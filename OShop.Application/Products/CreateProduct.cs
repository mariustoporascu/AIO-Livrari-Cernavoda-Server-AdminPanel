using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Products
{
    public class CreateProduct
    {
        private readonly ApplicationDbContext _context;

        public CreateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(ProductViewModel vm)
        {
            _context.Products.Add(new Product { 
                ProductId = vm.ProductId,
                Name = vm.Name,
                Description = vm.Description,
                Stock = vm.Stock,
                Price = vm.Price,
                Photo = vm.Photo,
                CategoryRefId = vm.CategoryRefId,
            });
            await _context.SaveChangesAsync();
        }
    }
    public class ProductViewModel
    {

        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string Photo { get; set; }

        public int CategoryRefId { get; set; }
    }
}
