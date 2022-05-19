using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class MeasuringUnit
    {
        [Key]
        [Column(Order = 1)]
        public int UnitId { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
