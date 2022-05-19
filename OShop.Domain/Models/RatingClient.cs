﻿namespace OShop.Domain.Models
{
    public class RatingClient
    {

        public string UserRefId { get; set; }
        public ApplicationUser Users { get; set; }
        public int OrderRefId { get; set; }
        public Order Orderz { get; set; }
        public int RatingDeLaRestaurant { get; set; }
        public int RatingDeLaSofer { get; set; }
    }
}