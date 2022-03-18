﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System.Collections.Generic;

namespace OShop.UI.Pages
{
    [AllowAnonymous]
    public class ViewProductModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileManager _fileManager;

        public ViewProductModel(OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _fileManager = fileManager;
        }

        [BindProperty]
        public ProductVMUI Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryVMUI> Categ { get; set; }

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

        public void OnGet(int productName)
        {
            ShoppingCartId = LoadCart().CartId;
            Products = new GetProduct(_context).Do(productName);
            Categ = new GetAllCategories(_context, _fileManager).Do();
        }
    }
}
