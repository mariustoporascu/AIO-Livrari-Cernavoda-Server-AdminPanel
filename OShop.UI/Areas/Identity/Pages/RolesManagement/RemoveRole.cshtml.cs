using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace OShop.UI.Areas.Identity.Pages.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class RemoveRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RemoveRoleModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public async Task<IActionResult> OnPost(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToPage("./ManageRoles");
                else
                {
                    Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "No role found");
            return RedirectToPage("./ManageRoles");
        }
    }
}
