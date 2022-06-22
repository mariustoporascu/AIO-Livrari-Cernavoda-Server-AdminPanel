using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.IO;
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

        public IEnumerable<ProductVMUI> Do()
        {

            return _context.Products.AsNoTracking().AsEnumerable()
                .Select(prod => new ProductVMUI
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Stock = prod.Stock,
                    Price = prod.Price,
                    Photo = prod.Photo,
                    Gramaj = prod.Gramaj,
                    IsAvailable = prod.IsAvailable,
                    MeasuringUnitId = prod.MeasuringUnitId,
                    SubCategoryRefId = prod.SubCategoryRefId,
                    ExtraProduse = _context.ExtraProduse.AsNoTracking().AsEnumerable().Where(extr => extr.ProductRefId == prod.ProductId),
                });
        }
        public IEnumerable<ProductVMUI> Do(int subcategoryId)
        {

            return _context.Products.AsNoTracking().AsEnumerable().Where(prod => prod.SubCategoryRefId == subcategoryId)
                .Select(prod => new ProductVMUI
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Stock = prod.Stock,
                    Price = prod.Price,
                    Photo = prod.Photo,
                    Gramaj = prod.Gramaj,
                    IsAvailable = prod.IsAvailable,
                    MeasuringUnitId = prod.MeasuringUnitId,
                    SubCategoryRefId = prod.SubCategoryRefId,
                });
        }
    }

}
