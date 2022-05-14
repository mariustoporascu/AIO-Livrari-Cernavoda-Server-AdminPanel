using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class LockOrder
    {
        private readonly OnlineShopDbContext _context;

        public LockOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Do(string driverId, int orderId)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId);
            if (order == null || order.DriverRefId != null)
                return false;
            order.DriverRefId = driverId;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
