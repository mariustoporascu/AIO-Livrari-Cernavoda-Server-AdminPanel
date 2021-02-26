using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System.Linq;

namespace OShop.Application.OrderInfos
{
    public class GetOrderInfo
    {
        private readonly OnlineShopDbContext _context;

        public GetOrderInfo(OnlineShopDbContext context)
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
