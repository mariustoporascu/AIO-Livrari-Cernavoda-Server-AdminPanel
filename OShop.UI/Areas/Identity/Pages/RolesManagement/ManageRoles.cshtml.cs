using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace OShop.UI.Areas.Identity.Pages.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class ManageRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IEnumerable<IdentityRole> RolesViewModels { get; set; }

        public void OnGet()
        {
            RolesViewModels = _roleManager.Roles.AsNoTracking().AsEnumerable();
        }
    }
}
