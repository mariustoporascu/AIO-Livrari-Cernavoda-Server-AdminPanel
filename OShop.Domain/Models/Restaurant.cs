using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class Restaurant
    {
        [Key]
        [Column(Order = 1)]
        public int RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; } = "";
        public string TelefonNo { get; set; } = "";
        public DateTime Opening { get; set; } = DateTime.UtcNow;
        [Range(0.01, 100.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TransporFee { get; set; }
        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MinimumOrderValue { get; set; }
        public bool IsActive { get; set; } = false;
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<RatingRestaurant> RatingRestaurants { get; set; }
    }
}
