﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LivroManage.UI.Pages.AdminPanel.RolesManagement
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