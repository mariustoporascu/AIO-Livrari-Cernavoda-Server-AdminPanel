using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class Order
    {
        [Key]
        [Column(Order = 1)]
        public int OrderId { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(0.01, 1000000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalOrdered { get; set; }
        public string CustomerId { get; set; }
        public string EstimatedTime { get; set; }
        public bool? HasUserConfirmedET { get; set; } = null;
        public bool IsRestaurant { get; set; }
        public int RestaurantRefId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        public OrderInfo OrderInfos { get; set; }
        public string? DriverRefId { get; set; }
        public ApplicationUser Driver { get; set; }
        public bool RestaurantGaveRating { get; set; } = false;
        public bool ClientGaveRatingDriver { get; set; } = false;
        public bool ClientGaveRatingRestaurant { get; set; } = false;
        public bool DriverGaveRating { get; set; } = false;
        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
        public virtual ICollection<RatingClient> RatingClients { get; set; }
        public virtual ICollection<RatingDriver> RatingDrivers { get; set; }
        public virtual ICollection<RatingRestaurant> RatingRestaurants { get; set; }
    }
}
