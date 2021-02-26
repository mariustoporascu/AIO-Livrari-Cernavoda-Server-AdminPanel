using Microsoft.AspNetCore.Identity;

namespace OShop.Database
{
    public class Enums : IdentityRole
    {
        public enum Roles
        {
            SuperAdmin,
            Admin,
            Moderator,
            Customer
        }
    }
}
