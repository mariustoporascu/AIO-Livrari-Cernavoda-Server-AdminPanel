using Microsoft.AspNetCore.Http;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Products
{
    public class CreateProduct
    {
        private readonly OnlineShopDbContext _context;

        public CreateProduct(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(ProductVMUI vm)
        {
            var product = new Product
            {
                ProductId = vm.ProductId,
                Name = vm.Name,
                Description = vm.Description,
                Stock = vm.Stock,
                Price = vm.Price,
                Photo = vm.Photo,
                Gramaj = vm.Gramaj,
                MeasuringUnitId = vm.MeasuringUnitId,
                SubCategoryRefId = vm.SubCategoryRefId,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
    public class ProductVMUI
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Gramaj { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public int MeasuringUnitId { get; set; }

        public string Photo { get; set; }

        public string Image { get; set; }
        public IEnumerable<ExtraProdus> ExtraProduse { get; set; }

        public int SubCategoryRefId { get; set; }
    }
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This photo extension is not allowed!";
        }
    }

}
