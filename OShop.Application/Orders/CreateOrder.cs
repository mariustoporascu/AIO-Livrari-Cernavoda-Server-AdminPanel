using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class CreateOrder
    {
        private readonly OnlineShopDbContext _context;

        public CreateOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Do(OrderViewModel vm)
        {
            var order = new Order
            {
                OrderId = vm.OrderId,
                Status = vm.Status,
                CustomerId = vm.CustomerId,
                TotalOrdered = vm.TotalOrdered,
                Created = DateTime.Now,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderId;
        }
    }
    public class OrderViewModel
    {

        public int OrderId { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(0.01, 1000000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalOrdered { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;
        public IEnumerable<ProductInOrdersViewModel> ProductsInOrder { get; set; }
        public OrderInfosViewModel OrderInfo { get; set; }
    }
}
