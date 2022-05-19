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
using OShop.Domain.Models;
using OShop.UI.ApiAuth;
using OShop.UI.Extras;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodAppController : ControllerBase
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public FoodAppController(OnlineShopDbContext context, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }
        partial class DriverLocation
        {
            public string Id { get; set; }
            public int OrderId { get; set; }
            public double CoordX { get; set; }
            public double CoordY { get; set; }
        }
        [HttpGet("getallproducts")]
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context, _fileManager).Do());

        [HttpGet("getallcategories")]
        public IActionResult ManageCategories() => Ok(new GetAllCategories(_context, _fileManager).Do());

        [HttpGet("getallsubcategories")]
        public IActionResult ManageSubCategories() => Ok(new GetAllSubCategories(_context, _fileManager).Do());

        [HttpGet("getallrestaurante")]
        public IActionResult ManageRestaurante() => Ok(new GetAllRestaurante(_context, _fileManager).Do());

        [HttpGet("getallsupermarkets")]
        public IActionResult ManageSuperMarkets() => Ok(new GetAllSuperMarkets(_context, _fileManager).Do());

        [HttpGet("getallmeasuringunits")]
        public IActionResult ManageMeasuringUnits() => Ok(new GetAllMeasuringUnits(_context).Do());

        [Authorize]
        [HttpGet("getallorders/{customer}")]
        public async Task<IActionResult> GetAllOrders(string customer) =>
            Ok(await new GetAllOrders(_context, _userManager).Do((await _userManager.FindByEmailAsync(customer)).Id));

        [Authorize]
        [HttpGet("getorderinfo/{orderId}")]
        public IActionResult GetOrderInfo(int orderId) => Ok(new GetOrderInfo(_context).Do(orderId));

        [Authorize]
        [HttpGet("getproductsinorder/{orderId}")]
        public IActionResult GetProductsInOrder(int orderId) => Ok(new GetAllProductInOrder(_context).Do(orderId));

        [Authorize]
        [HttpGet("getproductsfororder/{orderId}")]
        public IActionResult GetProductsForOrder(int orderId) => Ok(new GetAllProducts(_context, _fileManager).Do(0, orderId));

        [Authorize]
        [HttpGet("agreeesttime/{orderId}/{accept}")]
        public async Task<IActionResult> SetEstTime(int orderId, bool accept) => Ok($"agreed : {await new UpdateOrder(_context).DoET(orderId, accept)}");

        [Authorize]
        [HttpGet("getmydriverlocation/{driverId}/{orderId}")]
        public async Task<IActionResult> GetDriverLocation(string driverId, int orderId)
        {
            var driver = await _userManager.FindByIdAsync(driverId);
            var orderStatus = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId).Status;
            if (driver == null)
                return BadRequest();
            if (!orderStatus.Equals("In curs de livrare"))
                return BadRequest();
            var driverLocation = new DriverLocation
            {
                Id = driverId,
                OrderId = orderId,
                CoordX = driver.CoordX,
                CoordY = driver.CoordY,
            };
            return Ok(driverLocation);
        }

        [Authorize]
        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] object order)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var orderVM = JsonConvert.DeserializeObject<OrderViewModel>(order.ToString(), settings);
            var user = await _userManager.FindByEmailAsync(orderVM.CustomerId);
            if (user != null)
            {
                orderVM.Status = OrderStatusEnum.Plasata;
                orderVM.CustomerId = user.Id;
                int orderId = await new CreateOrder(_context).Do(orderVM);
                return Ok(orderId);
            }
            return Ok("User not found!");
        }

        [Authorize]
        [HttpPost("createorderinfo")]
        public async Task<IActionResult> CreateOrderInfo([FromBody] object orderInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var orderInfoVM = JsonConvert.DeserializeObject<OrderInfosViewModel>(orderInfo.ToString(), settings);
            await new CreateOrderInfo(_context).Do(orderInfoVM);
            return Ok();
        }

        [Authorize]
        [HttpPost("createorderproducts")]
        public async Task<IActionResult> CreateOrderProducts([FromBody] object orderproducts)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var orderProductsVM = JsonConvert.DeserializeObject<List<ProductInOrdersViewModel>>(orderproducts.ToString(), settings);
            await new CreateProductInOrder(_context).Do(orderProductsVM);
            return Ok();
        }

        [Authorize]
        [HttpGet("ratingdriver/{email}/{orderId}/{rating}")]
        public async Task<IActionResult> GiveDriverRating(string email, int orderId, int rating)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();

            var order = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (order == null) return BadRequest();
            var newRating = new RatingDriver
            {
                Rating = rating,
                OrderRefId = order.OrderId,
                DriverRefId = order.DriverRefId,
            };
            order.ClientGaveRatingDriver = true;
            _context.Orders.Update(order);
            _context.RatingDrivers.Add(newRating);
            await _context.SaveChangesAsync();
            return Ok("Rating acordat");
        }

        [Authorize]
        [HttpGet("ratingrestaurant/{email}/{orderId}/{rating}")]
        public async Task<IActionResult> GiveRestaurantRating(string email, int orderId, int rating)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();

            var order = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (order == null) return BadRequest();
            var newRating = new RatingRestaurant
            {
                Rating = rating,
                OrderRefId = order.OrderId,
                RestaurantRefId = order.RestaurantRefId,
            };
            order.ClientGaveRatingRestaurant = true;
            _context.Orders.Update(order);
            _context.RatingRestaurants.Add(newRating);
            await _context.SaveChangesAsync();
            return Ok("Rating acordat");
        }
    }
}
