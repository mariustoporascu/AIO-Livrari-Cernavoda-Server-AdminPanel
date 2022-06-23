using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
{
    public class Companie
    {
        [Key]
        [Column(Order = 1)]
        public int CompanieId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Name { get; set; }

        public string Photo { get; set; } = "";
        public string TelefonNo { get; set; } = "";
        public DateTime Opening { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = false;
        public bool TemporaryClosed { get; set; } = false;
        public bool VisibleInApp { get; set; } = false;
        public int TipCompanieRefId { get; set; }
        public TipCompanie TipCompanie { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<RatingCompanie> RatingCompanies { get; set; }

    }
}
