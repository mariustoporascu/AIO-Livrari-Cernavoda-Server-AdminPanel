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

        public OrderViewModel Do(int orderId)
        {
            return _context.Orders.AsNoTracking().Where(order => order.OrderId == orderId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                TransportFee = order.TransportFee,
                IsOrderPayed = order.IsOrderPayed,
                PaymentMethod = order.PaymentMethod,
                Comments = order.Comments,
                Created = order.Created,
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
            }).FirstOrDefault();

        }
    }
}
