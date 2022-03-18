using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OShop.UI.Areas.AdminPanel.Pages
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class OrderInfoModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public OrderInfoModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
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
            Products = new GetAllProducts(_context,_fileManager).Do(0,0)
                .Where(prod => ProductInOrders.Select(product => product.ProductRefId).Contains(prod.ProductId));
        }
    }
}
