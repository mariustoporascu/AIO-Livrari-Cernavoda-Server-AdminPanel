using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
{
    public class Order
    {
        [Key]
        [Column(Order = 1)]
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

        public OrderInfo OrderInfos { get; set; }
        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}
