using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class OrderOperations : EntityOperation<Order>
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderOperations(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<int> CreateInt(Order model)
        {

            _context.Orders.Add(model);
            await _context.SaveChangesAsync();
            return model.OrderId;
        }

        public override Task Create(Order model)
        {
            throw new NotImplementedException();
        }

        public override async Task Delete(int id)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(order => order.OrderId == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        public OrderVM GetVM(int? orderId)
        {
            return _context.Orders.AsNoTracking().AsEnumerable().Where(order => order.OrderId == orderId).Select(order => new OrderVM(order)
            {
                RatingDriver = _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.Rating ?? 0,
                RatingCompanie = _context.RatingCompanies.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.Rating ?? 0,
                RatingClientDeLaCompanie = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.RatingDeLaCompanie ?? 0,
                RatingClientDeLaSofer = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.RatingDeLaSofer ?? 0,
                ProductsInOrder = new ProductInOrderOperations(_context).GetAll(order.OrderId),
                OrderInfo = new OrderInfoOperations(_context).Get(order.OrderId),
            }).FirstOrDefault();

        }
        public override Order Get(int? id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Order> GetAll(int canal)
        {
            throw new NotImplementedException();
        }
        //driver
        public IEnumerable<OrderVM> GetAllVMDriver()
        {
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Select(order => new OrderVM(order)
            {
                RatingClientDeLaSofer = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.RatingDeLaSofer ?? 0,
                ProductsInOrder = new ProductInOrderOperations(_context).GetAll(order.OrderId),
                OrderInfo = new OrderInfoOperations(_context).Get(order.OrderId),
            }).ToList();
            foreach (var order in orders)
            {
                if (!order.TelephoneOrdered)
                    order.Location = GetUserLocation(order.UserLocationId);
                else
                    order.Location = GetTelLocation(order.OrderId);
            }
            return orders.AsEnumerable();
        }

        //client
        public async Task<IEnumerable<OrderVM>> GetAllVMClient(string customerId)
        {
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Where(order => order.CustomerId == customerId).Select(order => new OrderVM(order)
            {
                RatingDriver = _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.Rating ?? 0,
                RatingCompanie = _context.RatingCompanies.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.Rating ?? 0,
                ProductsInOrder = new ProductInOrderOperations(_context).GetAll(order.OrderId),
                OrderInfo = new OrderInfoOperations(_context).Get(order.OrderId),
            }).ToList();
            foreach (var order in orders)
            {
                if (!string.IsNullOrEmpty(order.DriverRefId))
                {
                    var driver = await _userManager.FindByIdAsync(order.DriverRefId);
                    order.Driver = new ApplicationUser
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        PhoneNumber = driver.PhoneNumber,
                    };
                }
            }
            return orders.AsEnumerable();
        }

        //owner
        public async Task<IEnumerable<OrderVM>> GetAllVMOwner(int companieId)
        {
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Where(order => order.CompanieRefId == companieId).Select(order => new OrderVM(order)
            {
                RatingClientDeLaCompanie = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.RatingDeLaCompanie ?? 0,
                ProductsInOrder = new ProductInOrderOperations(_context).GetAll(order.OrderId),
                OrderInfo = new OrderInfoOperations(_context).Get(order.OrderId),
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
                    order.Driver = new ApplicationUser
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        PhoneNumber = driver.PhoneNumber,
                    };
                }
            }
            return orders.AsEnumerable();
        }

        //site management
        public async Task<IEnumerable<OrderVM>> GetAllVMSite()
        {
            var orders = _context.Orders.AsNoTracking().AsEnumerable().Select(order => new OrderVM(order)
            {
                RatingDriver = _context.RatingDrivers.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.Rating ?? 0,
                RatingCompanie = _context.RatingCompanies.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.Rating ?? 0,
                RatingClientDeLaCompanie = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.RatingDeLaCompanie ?? 0,
                RatingClientDeLaSofer = _context.RatingClients.AsNoTracking().FirstOrDefault(ra => ra.OrderRefId == order.OrderId)?.RatingDeLaSofer ?? 0,
                ProductsInOrder = new ProductInOrderOperations(_context).GetAll(order.OrderId),
                OrderInfo = new OrderInfoOperations(_context).Get(order.OrderId),
            }).ToList();
            foreach (var order in orders)
            {
                if (!string.IsNullOrEmpty(order.DriverRefId))
                {
                    var driver = await _userManager.FindByIdAsync(order.DriverRefId);
                    order.Driver = new ApplicationUser
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        PhoneNumber = driver.PhoneNumber,
                    };
                }
            }
            return orders.AsEnumerable();
        }

        public override Task Update(Order model)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateBool(int orderId, string status)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;
            if (status == "In curs de livrare")
                order.StartDelivery = DateTime.UtcNow;
            if (status == "Livrata" || status == "Refuzata")
                order.FinishDelivery = DateTime.UtcNow;
            if (status == "Livrata")
                order.IsOrderPayed = true;
            ;
            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetEstTime(int orderId, string estimatedTime)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;
            if (order.TelephoneOrdered)
            {
                order.HasUserConfirmedET = true;
                order.Status = "In pregatire";
            }
            order.EstimatedTime = estimatedTime;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmEstTime(int orderId, bool accept)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;
            order.HasUserConfirmedET = accept;
            if (!accept)
                order.Status = "Anulata";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LockByDriver(string driverId, int orderId)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null || order.DriverRefId != null)
                return false;
            order.DriverRefId = driverId;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
        private UserLocations GetUserLocation(int locationId)
        {

            var userLocation = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.LocationId == locationId);
            if (userLocation == null)
                return new UserLocations();
            return new UserLocations
            {
                CoordX = userLocation.CoordX,
                CoordY = userLocation.CoordY,
            };
        }
        private UserLocations GetTelLocation(int orderId)
        {
            var userLocation = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == $"{orderId}");
            if (userLocation == null)
                return new UserLocations();
            return new UserLocations
            {
                CoordX = userLocation.CoordX,
                CoordY = userLocation.CoordY,
            };
        }
    }
}
