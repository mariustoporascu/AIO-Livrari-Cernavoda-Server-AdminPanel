using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
{
    public class UserLocations
    {
        [Key]
        [Column(Order = 1)]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string BuildingInfo { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
    }
}
