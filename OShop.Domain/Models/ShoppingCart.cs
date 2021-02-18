using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class ShoppingCart
    {
        [Key]
        [Column(Order = 1)]
        public int CartId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalInCart { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
