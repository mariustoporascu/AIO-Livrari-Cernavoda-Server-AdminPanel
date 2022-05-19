using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Application.Restaurante;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class CreateOrder
    {
        private readonly OnlineShopDbContext _context;

        public CreateOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Do(OrderViewModel vm)
        {
            var order = new Order
            {
                OrderId = vm.OrderId,
                Status = vm.Status,
                CustomerId = vm.CustomerId,
                TotalOrdered = vm.TotalOrdered,
                RestaurantRefId = vm.RestaurantRefId,
                IsRestaurant = vm.IsRestaurant,
                Created = DateTime.Now,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderId;
        }
    }
    public class OrderViewModel
    {

        public int OrderId { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(0.01, 1000000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalOrdered { get; set; }
        public string CustomerId { get; set; }
        public bool IsRestaurant { get; set; } = false;
        public int RestaurantRefId { get; set; }
        public string EstimatedTime { get; set; }
        public bool? HasUserConfirmedET { get; set; }
        public UserLocation Location { get; set; }
        public bool RestaurantGaveRating { get; set; } = false;
        public bool ClientGaveRatingDriver { get; set; } = false;
        public bool ClientGaveRatingRestaurant { get; set; } = false;
        public bool DriverGaveRating { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;
        public IEnumerable<ProductInOrdersViewModel> ProductsInOrder { get; set; }
        public OrderInfosViewModel OrderInfo { get; set; }
        public RestaurantVMUI Restaurant { get; set; }
        public string DriverRefId { get; set; }
        public Driver Driver { get; set; }
        public int RatingClientDeLaSofer { get; set; }
        public int RatingClientDeLaRestaurant { get; set; }
        public int RatingDriver { get; set; }
        public int RatingRestaurant { get; set; }
    }
    public class Driver
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelefonNo { get; set; }

    }
    public class UserLocation
    {
        public double CoordX { get; set; }
        public double CoordY { get; set; }
    }
}
