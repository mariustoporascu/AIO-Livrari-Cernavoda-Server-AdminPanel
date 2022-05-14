using Microsoft.EntityFrameworkCore;
using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
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

        public OrderViewModel Do(int orderId)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(order => order.OrderId == orderId);
            if (order == null)
                return null;
            else
                return new OrderViewModel
                {
                    OrderId = order.OrderId,
                    Status = order.Status,
                    CustomerId = order.CustomerId,
                    TotalOrdered = order.TotalOrdered,
                    IsRestaurant = order.IsRestaurant,
                    RestaurantRefId = order.RestaurantRefId,
                    ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                    OrderInfo = new GetOrderInfo(_context).Do(order.OrderId),
                    Created = order.Created,
                };
        }
    }
}
