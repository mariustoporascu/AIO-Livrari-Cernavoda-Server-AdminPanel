using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OShop.Application.CartItemsA;
using OShop.Application.Categories;
using OShop.Application.OrderInfos;
using OShop.Application.Orders;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OShop.Application.Products.GetAllProducts;

namespace OShop.UI.Areas.ShoppingCart.Pages
{
    [Authorize(Roles = "Customer")]
    public class LastCheckModel : PageModel
    {
        private readonly ILogger<LastCheckModel> _logger;
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LastCheckModel(ILogger<LastCheckModel> logger, OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryViewModel> Categ { get; set; }

        [BindProperty]
        public ShoppingCartViewModel ShoppingCart { get; set; }

        [BindProperty]
        public IEnumerable<CartItemsViewModel> CartItems { get; set; }

        [BindProperty]
        public OrderInfosViewModel OrderInfos { get; set; }

        public OrderViewModel Order { get; set; }

        public void OnGet()
        {
            var currUser = _userManager.GetUserId(User);
            ShoppingCart = new GetShoppingCart(_context).Do(currUser);
            CartItems = new GetCartItems(_context).Do(ShoppingCart.CartId);
            Products = new GetAllProducts(_context).Do()
                .Where(prod => CartItems.Select(cartItem => cartItem.ProductRefId)
                .Contains(prod.ProductId));
            Categ = new GetAllCategories(_context).Do();
            Order = new GetOrder(_context).Do(currUser, "Pending");
            OrderInfos = new GetOrderInfo(_context).Do(Order.OrderId);
        }
    }
}
