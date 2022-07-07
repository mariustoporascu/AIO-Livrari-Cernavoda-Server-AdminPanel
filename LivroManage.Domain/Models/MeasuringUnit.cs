using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivroManage.Domain.Models
{
    public class MeasuringUnit
    {
        [Key]
        [Column(Order = 1)]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Name { get; set; }

    }
}
