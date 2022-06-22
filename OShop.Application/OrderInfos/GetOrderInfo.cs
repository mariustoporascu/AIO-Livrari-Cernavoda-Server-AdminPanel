using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
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

        public OrderInfo Do(int orderId) => _context.OrdersInfos.AsNoTracking().FirstOrDefault(orderinfo => orderinfo.OrderRefId == orderId);

    }
}
