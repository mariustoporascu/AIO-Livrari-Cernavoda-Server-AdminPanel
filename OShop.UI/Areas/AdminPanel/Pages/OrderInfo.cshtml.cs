using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OShop.UI.Areas.AdminPanel.Pages
{
    [Authorize(Roles = "SuperAdmin")]
    public class OrderInfoModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public OrderInfoModel(OnlineShopDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public OrderInfosViewModel OrderInfo { get; set; }

        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }

        [BindProperty]
        public IEnumerable<ProductInOrdersViewModel> ProductInOrders { get; set; }

        public void OnGet(int orderId)
        {
            OrderInfo = new GetOrderInfo(_context).Do(orderId);
            ProductInOrders = new GetAllProductInOrder(_context).Do(orderId);
            Products = new GetAllProducts(_context).Do()
                .Where(prod => ProductInOrders.Select(product => product.ProductRefId).Contains(prod.ProductId));
        }
    }
}
