using OShop.Database;
using OShop.Domain.Models;
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
                FullName = vm.FullName,
                Address = vm.Address,
                PhoneNo = vm.PhoneNo,
                OrderRefId = vm.OrderRefId,
            };
            _context.OrdersInfos.Update(orderInfo);
            await _context.SaveChangesAsync();
        }
    }
}
