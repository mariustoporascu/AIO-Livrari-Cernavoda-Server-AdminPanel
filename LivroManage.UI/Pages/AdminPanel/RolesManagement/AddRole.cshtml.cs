using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnPost(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToPage("./ManageRoles");
        }
    }
}
