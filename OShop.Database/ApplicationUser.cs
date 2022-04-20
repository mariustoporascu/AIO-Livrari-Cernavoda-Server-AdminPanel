using Microsoft.AspNetCore.Identity;

namespace OShop.Database
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
    }
}
