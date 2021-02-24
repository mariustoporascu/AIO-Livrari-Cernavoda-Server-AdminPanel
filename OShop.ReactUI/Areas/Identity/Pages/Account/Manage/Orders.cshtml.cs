using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.ReactUI.Areas.Identity.Pages.Account.Manage
{
    public partial class OrdersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;

        public OrdersModel(OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public IEnumerable<OrderViewModel> Orders { get; set; }
        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }
        [BindProperty]
        public IEnumerable<ProductInOrdersViewModel> ProductInOrders { get; set; }

        private void LoadAsync(int orderId)
        {
            var userName = _userManager.GetUserId(User);
            Orders = new GetAllOrders(_context).Do(userName, orderId);
            if (orderId != -1)
            {
                ProductInOrders = new GetAllProductInOrder(_context).Do(Orders.FirstOrDefault(order => order.OrderId == orderId).OrderId);
                Products = new GetAllProducts(_context).Do()
                    .Where(prod => ProductInOrders.Select(product => product.ProductRefId).Contains(prod.ProductId));
            }
        }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user orders with ID '{_userManager.GetUserId(User)}'.");
            }
            if (orderId == 0)
                LoadAsync(-1);
            else
                LoadAsync(orderId);
            return Page();
        }
    }
}
