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
                RestaurantRefId = order.RestaurantRefId,
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
                RestaurantRefId = order.RestaurantRefId,
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
        //obsolete
        public IEnumerable<OrderViewModel> Do(string customerId, int orderId)
        {
            if (orderId == -1)
                return _context.Orders.AsNoTracking().Where(order => order.CustomerId == customerId && order.Status == "Ordered").ToList().Select(order => new OrderViewModel
                {
                    OrderId = order.OrderId,
                    Status = order.Status,
                    CustomerId = order.CustomerId,
                    TotalOrdered = order.TotalOrdered,
                    Created = order.Created,
                });
            return _context.Orders.AsNoTracking().Where(order => order.CustomerId == customerId && order.OrderId == orderId).ToList().Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                Created = order.Created,
            });
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
