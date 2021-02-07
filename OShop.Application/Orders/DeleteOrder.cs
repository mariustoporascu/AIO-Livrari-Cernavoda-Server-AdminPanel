using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class DeleteOrder
    {
        private readonly ApplicationDbContext _context;

        public DeleteOrder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(string customerId)
        {
            var order = _context.Orders.FirstOrDefault(order => order.CustomerId == customerId);
            await _context.SaveChangesAsync();
        }

        public async Task Do(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == orderId);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
