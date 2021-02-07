using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.OrderInfos
{
    public class GetOrderInfo
    {
        private readonly ApplicationDbContext _context;

        public GetOrderInfo(ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderInfosViewModel Do(int orderId)
        {
            var orderInfo = _context.OrdersInfos.AsNoTracking().FirstOrDefault(orderinfo => orderinfo.OrderRefId == orderId);
            if (orderInfo == null)
                return null;
            else
                return new OrderInfosViewModel
                {
                    OrderInfoId = orderInfo.OrderInfoId,
                    FirstName = orderInfo.FirstName,
                    LastName = orderInfo.LastName,
                    Address = orderInfo.Address,
                    PhoneNo = orderInfo.PhoneNo,
                    OrderRefId = orderInfo.OrderRefId,
                };
        }
    }
}
