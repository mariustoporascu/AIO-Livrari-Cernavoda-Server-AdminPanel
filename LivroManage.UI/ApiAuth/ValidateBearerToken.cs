using Microsoft.AspNetCore.Identity;
using LivroManage.Domain.Models;
using System.Threading.Tasks;

namespace LivroManage.UI.ApiAuth
{
    public class ValidateBearerToken
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ValidateBearerToken(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Validate(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            return (token == user.LoginToken);
        }
    }
}
