﻿using Microsoft.EntityFrameworkCore;
using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class GetAllOrders
    {
        private readonly OnlineShopDbContext _context;

        public GetAllOrders(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderViewModel> Do() =>
            _context.Orders.AsNoTracking().ToList().Where(order => order.Status != "Pending").Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                Created = order.Created,
            });
        public IEnumerable<OrderViewModel> Do(string customerId)
        => _context.Orders.AsNoTracking().ToList().Where(order => order.CustomerId == customerId).Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                TotalOrdered = order.TotalOrdered,
                Created = order.Created,
                ProductsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId),
                OrderInfo = new GetOrderInfo(_context).Do(order.OrderId)
            });


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
    }
}
