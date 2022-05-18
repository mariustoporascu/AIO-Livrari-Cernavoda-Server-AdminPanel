using Microsoft.EntityFrameworkCore;
using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Database;
using System.Linq;
using System.Threading.Tasks;

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
            return _context.Orders.AsNoTracking().Where(order => order.OrderId == orderId).Select(order => new OrderViewModel
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
            }).FirstOrDefault();
            
        }
    }
}
