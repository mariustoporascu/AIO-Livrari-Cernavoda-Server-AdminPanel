using Microsoft.AspNetCore.Identity;
using System;
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
        public bool CompleteProfile { get; set; } = false;
        public bool HasSetPassword { get; set; } = false;
        public string ResetTokenPass { get; set; } = "";
        public string ResetTokenPassIdentity { get; set; } = "";
        public int CompanieRefId { get; set; }
        public bool IsDriver { get; set; } = false;
        public bool IsOwner { get; set; } = false;

        public string LoginToken { get; set; }
        public DateTime LoginTokenExpiry { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Order> DriverOrders { get; set; }
        public virtual ICollection<RatingClient> RatingClients { get; set; }
        public virtual ICollection<RatingDriver> RatingDrivers { get; set; }
        public virtual ICollection<UserLocations> Locations { get; set; }
        public virtual ICollection<FireBaseTokens> FBTokens { get; set; }
    }
}
