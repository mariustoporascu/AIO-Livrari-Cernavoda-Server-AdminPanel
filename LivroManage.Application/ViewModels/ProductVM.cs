using LivroManage.Domain.Models;
using System.Collections.Generic;

namespace LivroManage.Application.ViewModels
{
    public class ProductVM : Product
    {
        public ProductVM(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            Description = product.Description;
            Stock = product.Stock;
            Price = product.Price;
            Photo = product.Photo;
            Gramaj = product.Gramaj;
            IsAvailable = product.IsAvailable;
            MeasuringUnitId = product.MeasuringUnitId;
            SubCategoryRefId = product.SubCategoryRefId;
        }
        public IEnumerable<ExtraProdus> ExtraProducts { get; set; }

    }
}
