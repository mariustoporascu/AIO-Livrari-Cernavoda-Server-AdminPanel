using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        public string Gramaj { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string Photo { get; set; } = "";
        [Required]
        public int MeasuringUnitId { get; set; }

        public int SubCategoryRefId { get; set; }
        public SubCategory SubCategory { get; set; }

        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
        public virtual ICollection<ExtraProdus> ExtraProduse { get; set; }
    }
}
