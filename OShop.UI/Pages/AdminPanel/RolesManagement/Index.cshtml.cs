using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OShop.UI.Pages.AdminPanel.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {

        }
        public void OnGet()
        {
        }
    }
}