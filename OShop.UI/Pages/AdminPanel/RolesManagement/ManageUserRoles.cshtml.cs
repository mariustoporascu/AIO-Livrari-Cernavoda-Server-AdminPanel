using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OShop.UI.Pages.AdminPanel.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class ManageUserRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUserRolesModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<UserRolesViewModel> UserRolesViewModels { get; set; }

        public class UserRolesViewModel
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public IEnumerable<string> Roles { get; set; }
        }

        public async Task OnGet()
        {
            var users = _userManager.Users.AsNoTracking().AsEnumerable().ToList();
            var userRoles = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(thisViewModel);
            }
            UserRolesViewModels = userRoles;
        }
    }
}
