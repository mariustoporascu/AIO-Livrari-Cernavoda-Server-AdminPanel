using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Pages
{
    [AllowAnonymous]
    public class ViewProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewProductModel(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public ProductViewModel Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryViewModel> Categ { get; set; }

        [BindProperty]
        public int ShoppingCartId { get; set; }

        public ShoppingCartViewModel LoadCart()
        {
            var currUser = _userManager.GetUserId(User);
            var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];
            if (currUser == null)
                return new GetShoppingCart(_context).Do(cookieValueFromContext);
            else
                return new GetShoppingCart(_context).Do(currUser);
        }

        public void OnGet(int productId)
        {
            ShoppingCartId = LoadCart().CartId;
            Products = new GetProduct(_context).Do(productId);
            Categ = new GetAllCategories(_context).Do();
        }
    }
}
