using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class UpdateOrder
    {
        private readonly OnlineShopDbContext _context;

        public UpdateOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(OrderViewModel vm)
        {
            var order = new Order
            {
                OrderId = vm.OrderId,
                Status = vm.Status,
                CustomerId = vm.CustomerId,
                TotalOrdered = vm.TotalOrdered,
                CompanieRefId = vm.CompanieRefId,
                Created = vm.Created,
            };
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> Do(int orderId, string status)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;
            if (status == "In curs de livrare")
                order.StartDelivery = DateTime.UtcNow;
            if (status == "Livrata" || status == "Refuzata")
                order.FinishDelivery = DateTime.UtcNow;
            if(status == "Livrata")
                order.IsOrderPayed = true;
;
            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DoET(int orderId, string estimatedTime)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;
            order.EstimatedTime = estimatedTime;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DoET(int orderId, bool accept)
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
    }
}
