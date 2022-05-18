using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.OrderInfos;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Application.Restaurante;
using OShop.Application.SubCategories;
using OShop.Application.SuperMarkets;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using OShop.Domain.Models;

namespace OShop.UI.Controllers
{
    [Route("api/[controller]")]
    public class FoodAppManageController : Controller
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        partial class DriverLocation
        {
            public string Id { get; set; }
            public double CoordX { get; set; }
            public double CoordY { get; set; }
        }
        public FoodAppManageController(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet("getalldriverorders")]
        public async Task<IActionResult> GetAllOrders() =>
            Ok(await new GetAllOrders(_context, _userManager).Do());
        [HttpGet("getallrestaurantorders/{restaurantRefId}")]
        public async Task<IActionResult> GetAllOrders(int restaurantRefId) =>
            Ok(await new GetAllOrders(_context, _userManager).Do(restaurantRefId));
        [HttpGet("updatestatus/{orderId}/{status}")]
        public async Task<IActionResult> OrderStatus(int orderId, string status)
        {

            if (await new UpdateOrder(_context).Do(orderId, status))
                return Ok("Order status updated.");
            return Ok("Order not found!");
        }
        [HttpPost("adjustproduct")]
        public async Task<IActionResult> AdjustProducts([FromBody] object orderProducts)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var orderProductsVM = JsonConvert.DeserializeObject<List<ProductInOrdersViewModel>>(orderProducts.ToString(), settings);
            decimal newTotal = 0.00M;
            int orderId = 0;
            foreach (var product in orderProductsVM)
            {
                if (orderId == 0)
                    orderId = product.OrderRefId;
                await new UpdateProductInOrder(_context).Do(product);
                newTotal += product.UsedQuantity * _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == product.ProductRefId).Price;
            }
            var order = new GetOrder(_context).Do(orderId);
            order.Status = OrderStatusEnum.Modificata;
            order.TotalOrdered = newTotal;
            await new UpdateOrder(_context).Do(order);
            return Ok(order);
        }
        [HttpGet("driverlockorder/{email}/{orderId}")]
        public async Task<IActionResult> DriverLockorder(string email, int orderId)
        {
            if (await new LockOrder(_context).Do((await _userManager.FindByEmailAsync(email)).Id, orderId))
                return Ok("Order locked.");
            return Ok("Order not locked.");
        }
        [HttpPost("driverupdatelocation")]
        public async Task<IActionResult> DriverUpdateLocation([FromBody] object driverLocation)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var driver = JsonConvert.DeserializeObject<DriverLocation>(driverLocation.ToString(), settings);
            var user = await _userManager.FindByIdAsync(driver.Id);
            if (user != null)
            {
                user.CoordX = driver.CoordX;
                user.CoordY = driver.CoordY;
                await _userManager.UpdateAsync(user);
                return Ok("Location updated");
            }
            return Ok("Location not updated");
        }
        [HttpGet("setesttime/{orderId}/{esttime}")]
        public async Task<IActionResult> SetEstTime(int orderId, string esttime) => Ok($"estTime : {await new UpdateOrder(_context).DoET(orderId, esttime)}");
        [HttpGet("ratingclient/{isOwner}/{orderId}/{rating}")]
        public async Task<IActionResult> GiveRestaurantRating(bool isOwner, int orderId, int rating)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (order == null) return BadRequest();
            var haveRating = _context.RatingClients.AsNoTracking().FirstOrDefault(rc => rc.OrderRefId == orderId);
            if (haveRating != null)
            {
                if (isOwner)
                    haveRating.RatingDeLaRestaurant = rating;
                else
                    haveRating.RatingDeLaSofer = rating;
                _context.RatingClients.Update(haveRating);
            }
            else
            {
                haveRating = new RatingClient
                {
                    OrderRefId = orderId,
                    UserRefId = order.CustomerId,
                };
                if (isOwner)
                    haveRating.RatingDeLaRestaurant = rating;
                else
                    haveRating.RatingDeLaSofer = rating;
                _context.RatingClients.Add(haveRating);

            }

            if (isOwner)
            {
                order.RestaurantGaveRating = true;
            }
            else
            {
                order.DriverGaveRating = true;
            }
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return Ok("Rating acordat");
        }
    }
}
