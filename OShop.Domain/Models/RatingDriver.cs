using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OShop.Domain.Models
{
    public class RatingDriver
    {

        public string DriverRefId { get; set; }
        public ApplicationUser Driver { get; set; }
        public int OrderRefId { get; set; }
        public Order Orderz { get; set; }
        public int Rating { get; set; }
    }
}
