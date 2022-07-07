using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LivroManage.UI.Pages.AdminPanel.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class ModifyUserRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ModifyUserRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public IList<ManageUserRolesVM> ManageUserRolesVMs { get; set; }

        public class ManageUserRolesVM
        {
            public int Index { get; set; }
            public string RoleId { get; set; }
            public string RoleName { get; set; }
            public bool Selected { get; set; }
        }

        public async Task OnGet(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            UserName = user.UserName;
            ManageUserRolesVMs = new List<ManageUserRolesVM>();
            var roles = _roleManager.Roles.ToList();
            int index = 0;
            foreach (var role in roles)
            {
                var manageUserRoles = new ManageUserRolesVM
                {
                    Index = index,
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                index += 1;
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageUserRoles.Selected = true;
                }
                else
                {
                    manageUserRoles.Selected = false;
                }
                ManageUserRolesVMs.Add(manageUserRoles);
            }
        }
        public async Task<IActionResult> OnPost(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage("./ManageUserRoles");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return RedirectToPage("./ModifyUserRoles");
            }
            result = await _userManager.AddToRolesAsync(user, ManageUserRolesVMs.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return RedirectToPage("./ModifyUserRoles");
            }
            return RedirectToPage("./ManageUserRoles");
        }
    }
}
