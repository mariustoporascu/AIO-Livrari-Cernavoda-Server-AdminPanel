using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OShop.UI.Areas.Identity.Pages.RolesManagement
{
    [Authorize(Roles = "SuperAdmin, Admin")]
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