using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivroManage.Domain.Models
{
    public class OrderInfo
    {
        [Key]
        [Column(Order = 1)]
        public int OrderInfoId { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string PhoneNo { get; set; }

        public int OrderRefId { get; set; }
        public Order Orders { get; set; }
    }
}
