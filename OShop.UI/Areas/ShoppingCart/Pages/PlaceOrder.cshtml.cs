using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OShop.Application.CartItemsA;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.OrderInfos;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
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
    public class PlaceOrderModel : PageModel
    {
        private readonly ILogger<PlaceOrderModel> _logger;
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileManager _fileManager;

        public PlaceOrderModel(ILogger<PlaceOrderModel> logger, OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager, IFileManager fileManager,
            SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _fileManager = fileManager;
        }

        public IEnumerable<ProductVMUI> Products { get; set; }

        public IEnumerable<CategoryViewModel> Categ { get; set; }

        public ShoppingCartViewModel ShoppingCart { get; set; }

        public IEnumerable<CartItemsViewModel> CartItems { get; set; }

        public OrderViewModel Order { get; set; }
        
        public ProductInOrdersViewModel ProductInOrders { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var currUser = _userManager.GetUserId(User);
            ShoppingCart = new GetShoppingCart(_context).Do(currUser);
            CartItems = new GetCartItems(_context).Do(ShoppingCart.CartId);
            Products = new GetAllProducts(_context).Do()
                .Where(prod => CartItems.Select(cartItem => cartItem.ProductRefId)
                .Contains(prod.ProductId));
            Order = new GetOrder(_context).Do(currUser, "Pending");
            foreach(var cartitem in CartItems.ToList())
            {
                await new CreateProductInOrder(_context).Do(new ProductInOrdersViewModel
                {
                    OrderRefId = Order.OrderId,
                    ProductRefId = cartitem.ProductRefId,
                    UsedQuantity = cartitem.Quantity,
                });
                await new UpdateProduct(_context, _fileManager).UpdateStockAfterOrder(cartitem.ProductRefId, cartitem.Quantity);
            }
            Order.Status = "Ordered";
            Order.TotalOrdered = ShoppingCart.TotalInCart;
            await new UpdateOrder(_context).Do(Order);
            ShoppingCart.Status = "Closed";
            await new UpdateShoppingCart(_context).Do(ShoppingCart);
            return RedirectToPage("/Index", new { Area = "" });
        }
    }
}
