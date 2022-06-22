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
        public IEnumerable<OrderViewModel> Do()
        {
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                TransportFee = order.TransportFee,
                IsOrderPayed = order.IsOrderPayed,
                TelephoneOrdered = order.TelephoneOrdered,
                Comments = order.Comments,
                UserLocationId = order.UserLocationId,
                PaymentMethod = order.PaymentMethod,
                EstimatedTime = order.EstimatedTime,
                HasUserConfirmedET = order.HasUserConfirmedET,
                DriverGaveRating = order.DriverGaveRating,
                CompanieRefId = order.CompanieRefId,
                RatingClientDeLaSofer = _context.RatingClients.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaSofer : 0,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                DriverRefId = order.DriverRefId,
                Created = order.Created,
            }).ToList();
            foreach (var order in orders)
            {
                if (!order.TelephoneOrdered)
                    order.Location = GetUserLocation(order.UserLocationId);
                else
                    order.Location = GetTelLocation(order.OrderId);
            }
            return orders;
        }

        //client
        public async Task<IEnumerable<OrderViewModel>> Do(string customerId)
        {
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Where(order => order.CustomerId == customerId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                TransportFee = order.TransportFee,
                Created = order.Created,
                IsOrderPayed = order.IsOrderPayed,
                PaymentMethod = order.PaymentMethod,
                Comments = order.Comments,
                EstimatedTime = order.EstimatedTime,
                UserLocationId = order.UserLocationId,

                ClientGaveRatingDriver = order.ClientGaveRatingDriver,
                ClientGaveRatingCompanie = order.ClientGaveRatingCompanie,
                RatingDriver = _context.RatingDrivers.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating : 0,
                RatingCompanie = _context.RatingCompanies.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingCompanies.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating : 0,
                CompanieRefId = order.CompanieRefId,
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
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Where(order => order.CompanieRefId == restaurantRefId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                TransportFee = order.TransportFee,
                Created = order.Created,
                IsOrderPayed = order.IsOrderPayed,
                TelephoneOrdered = order.TelephoneOrdered,
                PaymentMethod = order.PaymentMethod,
                Comments = order.Comments,
                UserLocationId = order.UserLocationId,

                EstimatedTime = order.EstimatedTime,
                HasUserConfirmedET = order.HasUserConfirmedET,
                CompanieGaveRating = order.CompanieGaveRating,
                CompanieRefId = order.CompanieRefId,
                RatingClientDeLaCompanie = _context.RatingClients.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaCompanie : 0,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                DriverRefId = order.DriverRefId,
            }).ToList();
            foreach (var order in orders)
            {
                if (!order.TelephoneOrdered)
                    order.Location = GetUserLocation(order.UserLocationId);
                else
                    order.Location = GetTelLocation(order.OrderId);
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
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                TransportFee = order.TransportFee,
                Created = order.Created,
                Comments = order.Comments,
                IsOrderPayed = order.IsOrderPayed,
                PaymentMethod = order.PaymentMethod,
                UserLocationId = order.UserLocationId,

                EstimatedTime = order.EstimatedTime,
                RatingDriver = _context.RatingDrivers.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating : 0,
                RatingCompanie = _context.RatingCompanies.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingCompanies.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).Rating : 0,
                RatingClientDeLaCompanie = _context.RatingClients.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaCompanie : 0,
                RatingClientDeLaSofer = _context.RatingClients.Count(ra => ra.OrderRefId == order.OrderId) > 0 ? _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId).RatingDeLaSofer : 0,
                CompanieRefId = order.CompanieRefId,
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
        private UserLocation GetUserLocation(int locationId)
        {

            var userLocation = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.LocationId == locationId);
            if (userLocation == null)
                return new UserLocation();
            return new UserLocation
            {
                CoordX = userLocation.CoordX,
                CoordY = userLocation.CoordY,
            };
        }
        private UserLocation GetTelLocation(int orderId)
        {
            var userLocation = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == $"{orderId}");
            if (userLocation == null)
                return new UserLocation();
            return new UserLocation
            {
                CoordX = userLocation.CoordX,
                CoordY = userLocation.CoordY,
            };
        }
    }
}
