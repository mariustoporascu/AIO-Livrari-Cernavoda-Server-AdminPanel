using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OShop.Application.CartItemsA;
using OShop.Application.Categories;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OShop.UI.Areas.ShoppingCart.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {

        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel( OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _userManager = userManager;

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

        public ShoppingCartViewModel LoadCart()
        {
            var currUser = _userManager.GetUserId(User);
            var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];
            if (currUser == null)
                return new GetShoppingCart(_context).Do(cookieValueFromContext);
            else
                return new GetShoppingCart(_context).Do(currUser);
        }

        public void OnGet()
        {
            ShoppingCart = LoadCart();
            CartItems = new GetCartItems(_context).Do(ShoppingCart.CartId);
            Products = new GetAllProducts(_context).Do()
                .Where(prod => CartItems.Select(cartItem => cartItem.ProductRefId)
                .Contains(prod.ProductId));
            Categ = new GetAllCategories(_context).Do();
        }
    }
}
