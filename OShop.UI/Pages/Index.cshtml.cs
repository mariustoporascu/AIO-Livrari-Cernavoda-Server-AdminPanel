using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IndexModel : PageModel
    {
        private readonly IFileManager _fileManager;
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(
            OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            IFileManager fileManager)
        {
            _fileManager = fileManager;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryVMUI> Categ { get; set; }

        [BindProperty]
        public int ProduseVandute { get; set; }
        [BindProperty]
        public int TotalCLienti { get; set; }
        [BindProperty]
        public int Comenzi { get; set; }
        [BindProperty]
        public int Rating { get; set; }
        [BindProperty]
        public decimal TotalVanzari { get; set; }

        //public async Task<ShoppingCartViewModel> LoadCart()
        //{
        //    var currUser = _userManager.GetUserId(User);
        //    var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];

        //    if (cookieValueFromContext != null)
        //    {
        //        var cookieCart = new GetShoppingCart(_context).Do(cookieValueFromContext);
        //        if (currUser != null && cookieCart != null)
        //        {
        //            var userCart = new GetShoppingCart(_context).Do(currUser);
        //            if (cookieCart.CustomerId != currUser && userCart == null)
        //            {
        //                cookieCart.CustomerId = currUser;
        //                await new UpdateShoppingCart(_context).Do(cookieCart);
        //                _httpContextAccessor.HttpContext.Response.Cookies.Delete("anonymousUsr");
        //                return cookieCart;
        //            }
        //            else
        //                return userCart;
        //        }
        //        else if (cookieCart == null)
        //        {
        //            await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
        //            {
        //                CustomerId = cookieValueFromContext,
        //            });
        //            return new GetShoppingCart(_context).Do(cookieValueFromContext);
        //        }
        //        else
        //            return cookieCart;
        //    }
        //    else if (currUser != null)
        //    {
        //        var usercart = new GetShoppingCart(_context).Do(currUser);
        //        if (usercart == null)
        //        {
        //            await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
        //            {
        //                CustomerId = currUser,
        //            });
        //            return new GetShoppingCart(_context).Do(currUser);
        //        }
        //        return usercart;
        //    }

        //    else
        //    {
        //        var userId = Guid.NewGuid().ToString();
        //        var option = new CookieOptions();
        //        option.Expires = DateTime.Now.AddDays(10);
        //        Response.Cookies.Append("anonymousUsr", userId, option);
        //        await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
        //        {
        //            CustomerId = userId,
        //        });
        //        return new GetShoppingCart(_context).Do(userId);
        //    }
        //}

        public async Task<IActionResult> OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var startTime = DateTime.UtcNow;
                var user = await _userManager.GetUserAsync(User);
                if (user.RestaurantRefId > 0)
                {
                    var orders = await new GetAllOrders(_context, _userManager).Do(user.RestaurantRefId);
                    Comenzi = orders.Count();
                    foreach (var order in orders)
                    {
                        var productsInOrder = new GetAllProductInOrder(_context).Do(order.OrderId).Select(po => po.UsedQuantity);
                        foreach (var product in productsInOrder)
                            ProduseVandute += product;
                        TotalVanzari += order.TotalOrdered;
                    }
                    TotalCLienti = orders.Select(or => or.CustomerId).Distinct().Count();
                    var ratings = _context.RatingRestaurants.AsNoTracking().AsEnumerable().Where(rat => rat.RestaurantRefId == user.RestaurantRefId).Select(ra => ra.Rating);
                    decimal sumRating = ratings.Count() * 5.0M;
                    decimal totalRating = 0.0M;
                    foreach (var rat in ratings)
                        totalRating += rat;
                    Rating = (int)Math.Abs(totalRating / sumRating * 100.0M);
                }
                else
                {
                    Comenzi = 0;
                    TotalCLienti = 0;
                    TotalVanzari = 0;
                    Rating = 0;
                }

                var timeEnd = DateTime.UtcNow;
                var runTime = timeEnd.Subtract(startTime).TotalSeconds;
                Console.WriteLine($"Total time : {runTime}");
                return Page();

            }
            return RedirectToPage("/Auth/Login");
            //ShoppingCartId = LoadCart().Result.CartId;
            //Products = new GetAllProducts(_context, _fileManager).Do(0, 0);
            //Categ = new GetAllCategories(_context, _fileManager).Do();
        }

        public void OnPost(string ProductName)
        {
            //ShoppingCartId = LoadCart().Result.CartId;
            //if (ProductName != null)
            //    Products = new GetAllProducts(_context, _fileManager).Do(0, 0).Where(prod => prod.Name.ToLower().Contains(ProductName.ToLower()));
            //else
            //    Products = new GetAllProducts(_context, _fileManager).Do(0, 0);
            //Categ = new GetAllCategories(_context, _fileManager).Do();
        }
    }
}
