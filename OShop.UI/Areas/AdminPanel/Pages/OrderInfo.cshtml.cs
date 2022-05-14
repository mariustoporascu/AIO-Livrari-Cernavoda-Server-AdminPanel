using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.OrderInfos;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public OrderViewModel Order { get; set; }

        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }
        [BindProperty]
        public IList<string> OrderStatuses { get; set; }

        [BindProperty]
        public IEnumerable<ProductInOrdersViewModel> ProductInOrders { get; set; }

        public void OnGet(int orderId)
        {
            Order = new GetOrder(_context).Do(orderId);
            OrderStatuses = new List<string>();
            OrderStatuses.Add(OrderStatusEnum.Plasata);
            OrderStatuses.Add(OrderStatusEnum.Preluata);
            OrderStatuses.Add(OrderStatusEnum.Modificata);
            OrderStatuses.Add(OrderStatusEnum.Pregatita);
            OrderStatuses.Add(OrderStatusEnum.SpreLivrare);
            OrderStatuses.Add(OrderStatusEnum.Livrata);
            OrderInfo = new GetOrderInfo(_context).Do(orderId);
            ProductInOrders = new GetAllProductInOrder(_context).Do(orderId);
            Products = new GetAllProducts(_context, _fileManager).Do(0, 0)
                .Where(prod => ProductInOrders.Select(product => product.ProductRefId).Contains(prod.ProductId));
        }
        public async Task OnPost()
        {
            await new UpdateOrder(_context).Do(Order);
            Order = new GetOrder(_context).Do(Order.OrderId);
            OrderStatuses = new List<string>();
            OrderStatuses.Add(OrderStatusEnum.Plasata);
            OrderStatuses.Add(OrderStatusEnum.Preluata);
            OrderStatuses.Add(OrderStatusEnum.Modificata);
            OrderStatuses.Add(OrderStatusEnum.Pregatita);
            OrderStatuses.Add(OrderStatusEnum.SpreLivrare);
            OrderStatuses.Add(OrderStatusEnum.Livrata);
            OrderInfo = new GetOrderInfo(_context).Do(Order.OrderId);
            ProductInOrders = new GetAllProductInOrder(_context).Do(Order.OrderId);
            Products = new GetAllProducts(_context, _fileManager).Do(0, 0)
                .Where(prod => ProductInOrders.Select(product => product.ProductRefId).Contains(prod.ProductId));
        }
    }
}
