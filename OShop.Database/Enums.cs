using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
