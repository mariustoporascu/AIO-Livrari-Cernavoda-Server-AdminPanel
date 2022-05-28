using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.Orders;
using OShop.Application.Products;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class OrderInfoModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;

        public OrderInfoModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public OrderViewModel Order { get; set; }
        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }


        public async Task<IActionResult> OnGet(int vm)
        {
            Order = new GetOrder(_context).Do(vm);
            var user = await _userManager.GetUserAsync(User);
            if (!(await _userManager.IsInRoleAsync(user, "SuperAdmin")) && Order.RestaurantRefId != user.RestaurantRefId)
                return RedirectToPage("/Error");
            Products = new GetAllProducts(_context).Do(0, 0)
                .Where(prod => Order.ProductsInOrder.Select(product => product.ProductRefId).Contains(prod.ProductId));
            return Page();
        }

    }
}
