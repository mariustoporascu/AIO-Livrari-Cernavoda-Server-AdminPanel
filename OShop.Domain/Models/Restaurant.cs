using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
