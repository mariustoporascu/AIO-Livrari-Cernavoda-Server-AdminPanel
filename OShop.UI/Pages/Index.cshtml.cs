using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OShop.Application.Categories;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(
            OnlineShopDbContext context,
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
        public IEnumerable<CategoryVMUI> Categ { get; set; }

        [BindProperty]
        public int ShoppingCartId { get; set; }

        public async Task<ShoppingCartViewModel> LoadCart()
        {
            var currUser = _userManager.GetUserId(User);
            var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];

            if (cookieValueFromContext != null)
            {
                var cookieCart = new GetShoppingCart(_context).Do(cookieValueFromContext);
                if (currUser != null && cookieCart != null)
                {
                    var userCart = new GetShoppingCart(_context).Do(currUser);
                    if (cookieCart.CustomerId != currUser && userCart == null)
                    {
                        cookieCart.CustomerId = currUser;
                        await new UpdateShoppingCart(_context).Do(cookieCart);
                        _httpContextAccessor.HttpContext.Response.Cookies.Delete("anonymousUsr");
                        return cookieCart;
                    }
                    else
                        return userCart;
                }
                else if (cookieCart == null)
                {
                    await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
                    {
                        CustomerId = cookieValueFromContext,
                    });
                    return new GetShoppingCart(_context).Do(cookieValueFromContext);
                }
                else
                    return cookieCart;
            }
            else if (currUser != null)
            {
                var usercart = new GetShoppingCart(_context).Do(currUser);
                if (usercart == null)
                {
                    await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
                    {
                        CustomerId = currUser,
                    });
                    return new GetShoppingCart(_context).Do(currUser);
                }
                return usercart;
            }

            else
            {
                var userId = Guid.NewGuid().ToString();
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Append("anonymousUsr", userId, option);
                await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
                {
                    CustomerId = userId,
                });
                return new GetShoppingCart(_context).Do(userId);
            }
        }

        public void OnGet()
        {
            ShoppingCartId = LoadCart().Result.CartId;
            Products = new GetAllProducts(_context).Do();
            Categ = new GetAllCategories(_context).Do();
        }

        public void OnPost(string ProductName)
        {
            ShoppingCartId = LoadCart().Result.CartId;
            if (ProductName != null)
                Products = new GetAllProducts(_context).Do().Where(prod => prod.Name.ToLower().Contains(ProductName.ToLower()));
            else
                Products = new GetAllProducts(_context).Do();
            Categ = new GetAllCategories(_context).Do();
        }
    }
}
