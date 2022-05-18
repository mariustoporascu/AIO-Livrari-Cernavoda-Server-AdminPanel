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
        private readonly IFileManager _fileManager;

        public GetAllProducts(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public IEnumerable<ProductVMUI> Do(int cartId, int orderId)
        {
            if (cartId > 0)
            {
                var cartItems = _context.CartItems.AsNoTracking().Where(cart => cart.CartRefId == cartId);

                return _context.Products.AsNoTracking().Where(prod => cartItems.Select(cartItem => cartItem.ProductRefId)
                    .Contains(prod.ProductId)).Select(prod => new ProductVMUI
                    {
                        ProductId = prod.ProductId,
                        Name = prod.Name,
                        Description = prod.Description,
                        Stock = prod.Stock,
                        Price = prod.Price,
                        Photo = prod.Photo,
                        Gramaj = prod.Gramaj,
                        MeasuringUnitId = prod.MeasuringUnitId,
                        CategoryRefId = prod.CategoryRefId,
                        SubCategoryRefId = prod.SubCategoryRefId,

                        SuperMarketRefId = prod.SuperMarketRefId,
                        RestaurantRefId = prod.RestaurantRefId,
                    });
            }
            if (orderId > 0)
            {
                var productsInOrder = _context.ProductInOrders.AsNoTracking()
                    .Where(prod => prod.OrderRefId == orderId);

                return _context.Products.AsNoTracking().Where(prod => productsInOrder
                    .Select(product => product.ProductRefId)
                    .Contains(prod.ProductId)).Select(prod => new ProductVMUI
                    {
                        ProductId = prod.ProductId,
                        Name = prod.Name,
                        Description = prod.Description,
                        Stock = prod.Stock,
                        Price = prod.Price,
                        Photo = prod.Photo,
                        Gramaj = prod.Gramaj,
                        MeasuringUnitId = prod.MeasuringUnitId,
                        CategoryRefId = prod.CategoryRefId,
                        SubCategoryRefId = prod.SubCategoryRefId,

                        SuperMarketRefId = prod.SuperMarketRefId,
                        RestaurantRefId = prod.RestaurantRefId,
                    });
            }
            return _context.Products.AsNoTracking().ToList().Select(prod => new ProductVMUI
            {
                ProductId = prod.ProductId,
                Name = prod.Name,
                Description = prod.Description,
                Stock = prod.Stock,
                Price = prod.Price,
                Photo = prod.Photo,
                Gramaj = prod.Gramaj,
                MeasuringUnitId = prod.MeasuringUnitId,
                CategoryRefId = prod.CategoryRefId,
                SubCategoryRefId = prod.SubCategoryRefId,

                SuperMarketRefId = prod.SuperMarketRefId,
                RestaurantRefId = prod.RestaurantRefId,
            });
        }
        public IEnumerable<ProductVMUI> Do()
        {

            return _context.Products.AsNoTracking()
                .ToList().Select(prod => new ProductVMUI
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Stock = prod.Stock,
                    Price = prod.Price,
                    Photo = prod.Photo,
                    //Image = (prod.Photo == null || prod.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(prod.Photo))),
                    Gramaj = prod.Gramaj,
                    MeasuringUnitId = prod.MeasuringUnitId,
                    CategoryRefId = prod.CategoryRefId,
                    SubCategoryRefId = prod.SubCategoryRefId,

                    SuperMarketRefId = prod.SuperMarketRefId,
                    RestaurantRefId = prod.RestaurantRefId,
                });
        }
        public IEnumerable<ProductVMUI> Do(int canal)
        {

            return _context.Products.AsNoTracking()
                .Where(prod => canal == 1 ? prod.SuperMarketRefId != null : prod.RestaurantRefId != null)
                .ToList().Select(prod => new ProductVMUI
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Stock = prod.Stock,
                    Price = prod.Price,
                    Photo = prod.Photo,
                    //Image = (prod.Photo == null || prod.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(prod.Photo))),
                    Gramaj = prod.Gramaj,
                    MeasuringUnitId = prod.MeasuringUnitId,
                    CategoryRefId = prod.CategoryRefId,
                    SubCategoryRefId = prod.SubCategoryRefId,

                    SuperMarketRefId = prod.SuperMarketRefId,
                    RestaurantRefId = prod.RestaurantRefId,
                });
        }
        public IEnumerable<ProductVMUI> DoRest(int canal)
        {

            return _context.Products.AsNoTracking()
                .Where(prod => prod.RestaurantRefId == canal)
                .Select(prod => new ProductVMUI
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Stock = prod.Stock,
                    Price = prod.Price,
                    Photo = prod.Photo,
                    //Image = (prod.Photo == null || prod.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(prod.Photo))),
                    Gramaj = prod.Gramaj,
                    MeasuringUnitId = prod.MeasuringUnitId,
                    CategoryRefId = prod.CategoryRefId,
                    SubCategoryRefId = prod.SubCategoryRefId,

                    SuperMarketRefId = prod.SuperMarketRefId,
                    RestaurantRefId = prod.RestaurantRefId,
                }).ToList();
        }
        private byte[] getBytes(FileStream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }

    }

}
