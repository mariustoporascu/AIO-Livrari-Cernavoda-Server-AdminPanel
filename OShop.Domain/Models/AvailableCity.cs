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

        [Required]
        public string Name { get; set; }
        public virtual ICollection<TransportFee> TransportFees { get; set; }
        public virtual ICollection<TransportFee> TransportFees2 { get; set; }

    }
}
