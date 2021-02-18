using OShop.Database;
using OShop.Domain.Models;
using System;
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

        public async Task Do(OrderViewModel vm)
        {
            _context.Orders.Add(new Order
            {
                OrderId = vm.OrderId,
                Status = vm.Status,
                CustomerId = vm.CustomerId,
                TotalOrdered = vm.TotalOrdered,
                Created = vm.Created,
            });
            await _context.SaveChangesAsync();
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
    }
}
