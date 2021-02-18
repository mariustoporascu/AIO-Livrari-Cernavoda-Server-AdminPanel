using OShop.Database;
using OShop.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace OShop.Application.ShoppingCarts
{
    public class CreateShoppingCart
    {
        private readonly OnlineShopDbContext _context;

        public CreateShoppingCart(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task Do(ShoppingCartViewModel vm)
        {
            _context.ShoppingCarts.Add(new ShoppingCart
            {
                CartId = vm.CartId,
                Status = "Active",
                CustomerId = vm.CustomerId,
                TotalInCart = vm.TotalInCart,
                Created = vm.Created,
            });
            await _context.SaveChangesAsync();
        }
    }
    public class ShoppingCartViewModel
    {
        public int CartId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalInCart { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
