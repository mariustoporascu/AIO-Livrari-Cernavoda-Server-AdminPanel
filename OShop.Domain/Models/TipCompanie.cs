using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
{
    public class TipCompanie
    {
        [Key]
        [Column(Order = 1)]
        public int TipCompanieId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; } = "";
        public bool IsOpen { get; set; } = false;
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public virtual ICollection<Companie> Companies { get; set; }

    }
}
