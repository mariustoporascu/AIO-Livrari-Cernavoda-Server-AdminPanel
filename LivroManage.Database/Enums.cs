using Microsoft.AspNetCore.Identity;

namespace LivroManage.Database
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
