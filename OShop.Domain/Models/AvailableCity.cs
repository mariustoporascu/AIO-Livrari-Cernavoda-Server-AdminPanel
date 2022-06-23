using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class AvailableCity
    {
        [Key]
        [Column(Order = 1)]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Name { get; set; }
        public bool IsAvailable { get; set; } = false;
        public virtual ICollection<TransportFee> TransportFees { get; set; }

    }
}
