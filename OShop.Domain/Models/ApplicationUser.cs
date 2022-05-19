using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OShop.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public string ProfilePicture { get; set; } = "";
        public string UserIdentification { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string BuildingInfo { get; set; }
        public bool CompleteProfile { get; set; } = false;
        public int RestaurantRefId { get; set; }
        public bool IsDriver { get; set; } = false;
        public bool IsOwner { get; set; } = false;
        public double CoordX { get; set; }
        public double CoordY { get; set; }
        public string LoginToken { get; set; }
        public virtual ICollection<Order> DriverOrders { get; set; }
        public virtual ICollection<RatingClient> RatingClients { get; set; }
        public virtual ICollection<RatingDriver> RatingDrivers { get; set; }
    }
}
