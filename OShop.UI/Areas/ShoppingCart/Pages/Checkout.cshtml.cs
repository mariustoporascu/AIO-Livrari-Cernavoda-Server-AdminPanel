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

namespace OShop.UI.Areas.ShoppingCart.Pages
{
    [Authorize(Roles = "Customer")]
    public class CheckoutModel : PageModel
    {
        private readonly ILogger<CheckoutModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutModel(ILogger<CheckoutModel> logger, ApplicationDbContext context,
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
        public IEnumerable<ProductViewModel> Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryViewModel> Categ { get; set; }

        [BindProperty]
        public ShoppingCartViewModel ShoppingCart { get; set; }

        [BindProperty]
        public IEnumerable<CartItemsViewModel> CartItems { get; set; }

        [BindProperty]
        public OrderInfosViewModel OrderInfos { get; set; }

        public OrderViewModel Order { get; set; }

        public async Task<ShoppingCartViewModel> LoadCart(string currUser)
        {
            var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];
            var usercart = new GetShoppingCart(_context).Do(currUser);
            if (cookieValueFromContext != null && usercart == null)
            {
                usercart = new GetShoppingCart(_context).Do(cookieValueFromContext);
                usercart.CustomerId = currUser;
                await new UpdateShoppingCart(_context).Do(usercart);
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("anonymousUsr");
                return usercart;
            }
            return usercart;
        }

        public async Task OnGet()
        {
            var currUser = _userManager.GetUserId(User);
            ShoppingCart = await LoadCart(currUser);
            CartItems = new GetCartItems(_context).Do(ShoppingCart.CartId);
            Products = new GetAllProducts(_context).Do()
                .Where(prod => CartItems.Select(cartItem => cartItem.ProductRefId)
                .Contains(prod.ProductId));
            Categ = new GetAllCategories(_context).Do();
            Order = new GetOrder(_context).Do(currUser, "Pending");
            if (Order == null)
            {
                await new CreateOrder(_context).Do(new OrderViewModel
                {
                    Status = "Pending",
                    CustomerId = currUser,
                    TotalOrdered = ShoppingCart.TotalInCart,
                });
                Order = new GetOrder(_context).Do(currUser, "Pending");
            }
            OrderInfos = new GetOrderInfo(_context).Do(Order.OrderId);
            if (OrderInfos == null)
                OrderInfos = new OrderInfosViewModel();
        }

        public async Task<IActionResult> OnPost()
        {
            var currUser = _userManager.GetUserId(User);
            Order = new GetOrder(_context).Do(currUser, "Pending");
            var orderInfo = new GetOrderInfo(_context).Do(Order.OrderId);
            if(orderInfo == null)
            {
                await new CreateOrderInfo(_context).Do(new OrderInfosViewModel
                {
                    FirstName = OrderInfos.FirstName,
                    LastName = OrderInfos.LastName,
                    Address = OrderInfos.Address,
                    PhoneNo = OrderInfos.PhoneNo,
                    OrderRefId = Order.OrderId,
                });
            }
            else
            {
                await new UpdateOrderInfo(_context).Do(new OrderInfosViewModel {
                    OrderInfoId = orderInfo.OrderInfoId,
                    FirstName = OrderInfos.FirstName,
                    LastName = OrderInfos.LastName,
                    Address = OrderInfos.Address,
                    PhoneNo = OrderInfos.PhoneNo,
                    OrderRefId = Order.OrderId,
                });
            }
            return RedirectToPage("/LastCheck");
        }
    }
}
