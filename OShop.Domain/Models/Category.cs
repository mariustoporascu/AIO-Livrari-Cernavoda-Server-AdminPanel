using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class Category
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; } = "";

        public virtual ICollection<Product> Products { get; set; }
    }
}
