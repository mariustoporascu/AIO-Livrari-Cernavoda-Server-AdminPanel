using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.OrderInfos
{
    public class UpdateOrderInfo
    {
        private readonly OnlineShopDbContext _context;

        public UpdateOrderInfo(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(OrderInfosViewModel vm)
        {
            var orderInfo = new OrderInfo
            {
                OrderInfoId = vm.OrderInfoId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Address = vm.Address,
                PhoneNo = vm.PhoneNo,
                OrderRefId = vm.OrderRefId,
            };
            _context.OrdersInfos.Update(orderInfo);
            await _context.SaveChangesAsync();
        }
    }
}
