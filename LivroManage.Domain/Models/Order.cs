using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivroManage.Domain.Models
{
    public class Order
    {
        [Key]
        [Column(Order = 1)]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Status { get; set; }

        [Range(0.01, 1000000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalOrdered { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TransportFee { get; set; }
        public string CustomerId { get; set; }
        public string EstimatedTime { get; set; }
        public bool TelephoneOrdered { get; set; } = false;
        public bool? HasUserConfirmedET { get; set; } = null;
        public string PaymentMethod { get; set; }
        public bool IsOrderPayed { get; set; } = false;
        public int CompanieRefId { get; set; }
        public int UserLocationId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDelivery { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FinishDelivery { get; set; }
        public string Comments { get; set; } = "";

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        public OrderInfo OrderInfos { get; set; }
        public string DriverRefId { get; set; }
        public ApplicationUser Driver { get; set; }
        public bool CompanieGaveRating { get; set; } = false;
        public bool ClientGaveRatingDriver { get; set; } = false;
        public bool ClientGaveRatingCompanie { get; set; } = false;
        public bool DriverGaveRating { get; set; } = false;
        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
        public virtual ICollection<RatingClient> RatingClients { get; set; }
        public virtual ICollection<RatingDriver> RatingDrivers { get; set; }
        public virtual ICollection<RatingCompanie> RatingCompanies { get; set; }
    }
}
