using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.OrderInfos
{
    public class CreateOrderInfo
    {
        private readonly OnlineShopDbContext _context;

        public CreateOrderInfo(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(OrderInfosViewModel vm)
        {
            _context.OrdersInfos.Add(new OrderInfo
            {
                OrderInfoId = vm.OrderInfoId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Address = vm.Address,
                PhoneNo = vm.PhoneNo,
                OrderRefId = vm.OrderRefId,
            });
            await _context.SaveChangesAsync();
        }
    }
    public class OrderInfosViewModel
    {
        public int OrderInfoId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNo { get; set; }

        public int OrderRefId { get; set; }
    }
}
