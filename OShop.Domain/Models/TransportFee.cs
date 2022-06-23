using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
{
    public class TransportFee
    {
        [Key]
        [Column(Order = 1)]
        public int TranspFeeId { get; set; }
        public int MainCityRefId { get; set; }
        public int CityRefId { get; set; }
        public AvailableCity AvailableCities { get; set; }
        public int TipCompanieRefId { get; set; }

        [Range(0.01, 100.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TransporFee { get; set; }
        [Range(0.01, 10000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MinimumOrderValue { get; set; }
    }
}
