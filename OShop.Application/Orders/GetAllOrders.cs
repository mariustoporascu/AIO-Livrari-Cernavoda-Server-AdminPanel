using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class GetAllOrders
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllOrders(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //driver
        public async Task<IEnumerable<OrderViewModel>> Do()
        {
            var orders = _context.Orders.AsNoTracking().Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                IsRestaurant = order.IsRestaurant,
                EstimatedTime = order.EstimatedTime,
                HasUserConfirmedET = order.HasUserConfirmedET,
                DriverGaveRating = order.DriverGaveRating,
                RestaurantRefId = order.RestaurantRefId,
                RatingClientDeLaSofer = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaSofer,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                DriverRefId = order.DriverRefId,
                Created = order.Created,
            }).ToList();
            foreach (var order in orders)
            {
                order.Location = await GetUserLocation(order.CustomerId);
            }
            return orders;
        }

        //client
        public async Task<IEnumerable<OrderViewModel>> Do(string customerId)
        {
            var orders = _context.Orders.AsNoTracking().Where(order => order.CustomerId == customerId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                Created = order.Created,
                EstimatedTime = order.EstimatedTime,
                IsRestaurant = order.IsRestaurant,
                ClientGaveRatingDriver = order.ClientGaveRatingDriver,
                ClientGaveRatingRestaurant = order.ClientGaveRatingRestaurant,
                RatingDriver = _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating,
                RatingRestaurant = _context.RatingRestaurants.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating,
                RestaurantRefId = order.RestaurantRefId,
                HasUserConfirmedET = order.HasUserConfirmedET,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                DriverRefId = order.DriverRefId,
            }).ToList();
            foreach (var order in orders)
            {
                if (!string.IsNullOrEmpty(order.DriverRefId))
                {
                    var driver = await _userManager.FindByIdAsync(order.DriverRefId);
                    order.Driver = new Driver
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        TelefonNo = driver.PhoneNumber,
                    };
                }
            }
            return orders;
        }
        //owner
        public async Task<IEnumerable<OrderViewModel>> Do(int restaurantRefId)
        {
            var orders = _context.Orders.AsNoTracking().Where(order => order.RestaurantRefId == restaurantRefId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                Created = order.Created,
                IsRestaurant = order.IsRestaurant,
                EstimatedTime = order.EstimatedTime,
                HasUserConfirmedET = order.HasUserConfirmedET,
                RestaurantGaveRating = order.RestaurantGaveRating,
                RestaurantRefId = order.RestaurantRefId,
                RatingClientDeLaRestaurant = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaRestaurant,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                DriverRefId = order.DriverRefId,
            }).ToList();
            foreach (var order in orders)
            {
                order.Location = await GetUserLocation(order.CustomerId);

                if (!string.IsNullOrEmpty(order.DriverRefId))
                {
                    var driver = await _userManager.FindByIdAsync(order.DriverRefId);
                    order.Driver = new Driver
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        TelefonNo = driver.PhoneNumber,
                    };
                }
            }
            return orders;
        }
        //site management
        public async Task<IEnumerable<OrderViewModel>> Do(string customerId, bool manager)
        {
            var orders = _context.Orders.AsNoTracking().Where(order => order.CustomerId == customerId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                Created = order.Created,
                EstimatedTime = order.EstimatedTime,
                IsRestaurant = order.IsRestaurant,
                RatingDriver = _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating,
                RatingRestaurant = _context.RatingRestaurants.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating,
                RatingClientDeLaRestaurant = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaRestaurant,
                RatingClientDeLaSofer = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaSofer,
                RestaurantRefId = order.RestaurantRefId,
                HasUserConfirmedET = order.HasUserConfirmedET,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                DriverRefId = order.DriverRefId,
            }).ToList();
            foreach (var order in orders)
            {
                if (!string.IsNullOrEmpty(order.DriverRefId))
                {
                    var driver = await _userManager.FindByIdAsync(order.DriverRefId);
                    order.Driver = new Driver
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        TelefonNo = driver.PhoneNumber,
                    };
                }
            }
            return orders;
        }
        private async Task<UserLocation> GetUserLocation(string customerId)
        {
            var user = await _userManager.FindByIdAsync(customerId);
            return new UserLocation
            {
                CoordX = user.CoordX,
                CoordY = user.CoordY,
            };
        }
    }
}
