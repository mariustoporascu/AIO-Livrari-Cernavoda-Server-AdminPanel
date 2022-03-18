using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
