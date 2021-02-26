using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.Orders
{
    public class GetOrder
    {
        private readonly OnlineShopDbContext _context;

        public GetOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public OrderViewModel Do(string customerId, string status)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(order => order.CustomerId == customerId && order.Status == status);
            if (order == null)
                return null;
            else
                return new OrderViewModel
                {
                    OrderId = order.OrderId,
                    Status = order.Status,
                    CustomerId = order.CustomerId,
                    TotalOrdered = order.TotalOrdered,
                    Created = order.Created,
                };
        }

        public OrderViewModel Do(int orderId, string status)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(order => order.OrderId == orderId && order.Status == status || order.OrderId == orderId);
            if (order == null)
                return null;
            else
                return new OrderViewModel
                {
                    OrderId = order.OrderId,
                    Status = order.Status,
                    CustomerId = order.CustomerId,
                    TotalOrdered = order.TotalOrdered,
                    Created = order.Created,
                };
        }
    }
}
