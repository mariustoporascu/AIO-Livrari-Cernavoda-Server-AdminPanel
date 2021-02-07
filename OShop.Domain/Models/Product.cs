using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
{
    public class Product
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string Photo { get; set; } = "";

        public int CategoryRefId { get; set; }
        public Category Categories { get; set; }

        public virtual ICollection<CartItems> CartItems { get; set; }
        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}
