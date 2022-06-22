using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class ExtraProdus
    {
        [Key]
        [Column(Order = 1)]
        public int ExtraProdusId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Name { get; set; }
        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int ProductRefId { get; set; }
        public Product Products { get; set; }
    }
}
