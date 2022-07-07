using Microsoft.AspNetCore.Identity;
using LivroManage.Domain.Models;
using System.Threading.Tasks;

namespace LivroManage.UI.ApiAuthManage
{
    public class ValidateBearerTokenManage
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ValidateBearerTokenManage(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Validate(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.CompanieRefId == 0)
                return false;
            return (token == user.LoginToken);
        }
    }
}
