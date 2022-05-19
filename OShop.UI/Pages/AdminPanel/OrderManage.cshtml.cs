using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Application.Orders;
using OShop.Database;
using OShop.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class OrderManageModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderManageModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<OrderViewModel> Orders { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.RestaurantRefId > 0)
                Orders = await new GetAllOrders(_context, _userManager).Do(user.RestaurantRefId);
            else
                Orders = await new GetAllOrders(_context, _userManager).Do(null, true);
            return Page();
        }
    }
}