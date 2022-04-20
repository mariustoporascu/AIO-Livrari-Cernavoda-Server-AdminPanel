using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class OrderInfo
    {
        [Key]
        [Column(Order = 1)]
        public int OrderInfoId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNo { get; set; }

        public int OrderRefId { get; set; }
        public Order Orders { get; set; }
    }
}
