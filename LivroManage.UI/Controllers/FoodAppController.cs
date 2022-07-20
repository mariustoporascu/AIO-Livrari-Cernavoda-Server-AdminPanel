using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Database;
using LivroManage.Domain.Models;
using LivroManage.UI.ApiAuth;
using LivroManage.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class FoodAppController : ControllerBase
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileManager _fileManager;
        private readonly string OneSignalApiKey;
        private readonly string OneSignalAppId;
        private readonly string FBLoginIosEnabled;
        private readonly IConfiguration _config;
        public FoodAppController(IConfiguration config, IFileManager fileManager, OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _fileManager = fileManager;
            OneSignalApiKey = config["ConnectionStrings:SignalApiKey"];
            OneSignalAppId = config["ConnectionStrings:SignalAppId"];
            FBLoginIosEnabled = config["ConnectionStrings:FBLoginIosEnabled"];
        }


        [HttpGet("getallproducts")]
        public IActionResult GetProducts() => Ok(new ProductOperations(_context, _fileManager).GetAllVM());

        [HttpGet("getallcategories")]
        public IActionResult GetCategories() => Ok(new CategoryOperations(_context, _fileManager).GetAll());

        [HttpGet("getallcities")]
        public IActionResult GetCities() => Ok(_context.AvailableCities.AsNoTracking().AsEnumerable().Where(city => city.IsAvailable == true));

        [HttpGet("getallsubcategories")]
        public IActionResult GetSubCategories() => Ok(new SubCategoryOperations(_context, _fileManager).GetAll());

        [HttpGet("fbbtnios")]
        public IActionResult FbIos() => Ok(FBLoginIosEnabled);

        [HttpGet("getpaymentmethods")]
        public IActionResult PaymentMethods() => Ok(new List<string>() { "Cash la livrare" });

        [HttpGet("getallcompanii")]
        public IActionResult GetCompanies() => Ok(new CompanieOperations(_context, _fileManager).GetAllVM().Where(comp => comp.VisibleInApp == true));

        [HttpGet("getalltipcompanii")]
        public IActionResult GetTipCompanii() => Ok(_context.TipCompanies.AsNoTracking().AsEnumerable());

        [HttpGet("getallmeasuringunits")]
        public IActionResult GetMeasuringUnits() => Ok(_context.MeasuringUnits.AsNoTracking().AsEnumerable());

        [Authorize]
        [HttpGet("getallorders/{customer}")]
        public async Task<IActionResult> GetAllOrders(string customer) =>
            Ok(await new OrderOperations(_context, _userManager).GetAllVMClient((await _userManager.FindByEmailAsync(customer)).Id));

        [Authorize]
        [HttpGet("getorderinfo/{orderId}")]
        public IActionResult GetOrderInfo(int orderId) => Ok(new OrderOperations(_context, _userManager).GetVM(orderId));

        [Authorize]
        [HttpGet("getproductsinorder/{orderId}")]
        public IActionResult GetProductsInOrder(int orderId) => Ok(new ProductInOrderOperations(_context).GetAll(orderId));

        [Authorize]
        [HttpGet("agreeesttime/{orderId}&{accept}")]
        public async Task<IActionResult> SetEstTime(int orderId, bool accept)
        {
            var orderVM = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (!orderVM.TelephoneOrdered)
            {
                var restaurant = _userManager.Users.FirstOrDefault(us => us.CompanieRefId == orderVM.CompanieRefId);
                if (restaurant != null)
                {
                    var restaurantToken = _context.FBTokens.AsNoTracking().AsEnumerable().Where(tkn => tkn.UserId == restaurant.Id)?
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (restaurantToken != null && restaurantToken.Count > 0)
                    {
                        var notifSender = new NotificationSender();
                        foreach (var token in restaurantToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, accept ? "Timpul estimat a fost acceptat!" : "Timpul estimat nu a fost acceptat. Poti anula comanda!");
                    }

                }
            }

            return Ok($"agreed : {await new OrderOperations(_context, _userManager).ConfirmEstTime(orderId, accept)}");
        }

        [Authorize]
        [HttpGet("getmydriverlocation/{driverId}&{orderId}")]
        public async Task<IActionResult> GetDriverLocation(string driverId, int orderId)
        {
            var driver = await _userManager.FindByIdAsync(driverId);
            var orderStatus = _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == orderId).Status;
            if (driver == null)
                return BadRequest();
            if (!orderStatus.Equals("In curs de livrare"))
                return BadRequest();
            var driverLocation = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == driverId);
            return Ok(driverLocation);
        }
        partial class HelpMe
        {
            public string Email { get; set; }
            public string TelNo { get; set; }
            public string Name { get; set; }
            public string Message { get; set; }
        }
        partial class ServerOrder : Order
        {
            public int RatingDriver { get; set; }
            public int RatingCompanie { get; set; }
            [JsonProperty("orderLocation")]
            public UserLocations UserLocation { get; set; }
            [JsonProperty("productsInOrder")]
            public List<ProductInOrder> ProductsInOrder { get; set; }
            [JsonProperty("orderInfo")]
            public OrderInfo OrderInfo { get; set; }

        }
        [Authorize]
        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] object orders)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var order = JsonConvert.DeserializeObject<ServerOrder>(orders.ToString(), settings);
            var user = await _userManager.FindByEmailAsync(order.CustomerId);
            if (user != null)
            {
                if (!order.TelephoneOrdered)
                {
                    order.CustomerId = user.Id;
                    order.Status = "Plasata";
                }
                else
                {
                    order.EstimatedTime = order.EstimatedTime;
                    order.HasUserConfirmedET = true;
                    order.Status = "In pregatire";
                    var drivers = _userManager.Users.Where(us => us.IsDriver == true).ToList();
                    foreach (var driver in drivers)
                    {
                        var driverToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == driver.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                        if (driverToken != null)
                        {
                            var notifSender = new NotificationSender();
                            foreach (var token in driverToken)
                                await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"A aparut o noua comanda fara livrator!");
                        }
                    }
                }
                int orderId = await new OrderOperations(_context, _userManager).CreateInt(order);
                if (order.TelephoneOrdered)
                {
                    var location = new UserLocations
                    {
                        UserId = $"{orderId}",
                        BuildingInfo = order.UserLocation.BuildingInfo,
                        Street = order.UserLocation.Street,
                        City = order.UserLocation.City,
                        CoordX = order.UserLocation.CoordX,
                        CoordY = order.UserLocation.CoordY,
                    };
                    _context.UserLocations.Add(location);
                    await _context.SaveChangesAsync();
                }
                order.OrderInfo.OrderRefId = orderId;
                await new OrderInfoOperations(_context).Create(order.OrderInfo);
                foreach (var product in order.ProductsInOrder)
                    product.OrderRefId = orderId;
                await new ProductInOrderOperations(_context).CreateList(order.ProductsInOrder);
                var restaurant = _userManager.Users.FirstOrDefault(us => us.CompanieRefId == order.CompanieRefId);
                if (restaurant != null && !order.TelephoneOrdered)
                {
                    var restaurantToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == restaurant.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (restaurantToken != null)
                    {
                        var notifSender = new NotificationSender();
                        foreach (var token in restaurantToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"Ai primit o noua comanda in sistem cu numarul {orderId}!");
                    }
                }
                return Ok("Order placed.");


            }
            return Ok("User not found!");
        }

        [HttpPost("askhelp")]
        public IActionResult SendHelpMsg([FromBody] object helpMsg)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var help = JsonConvert.DeserializeObject<HelpMe>(helpMsg.ToString(), settings);
            var sender = new EmailSender(_config);
            if (sender.SendEmail("support@livro.ro", $"Mesaj din applicatie de la : {help.Email}",
                $"{help.Name}, {help.TelNo}, {help.Message}"))
                return Ok("Message sent!");

            return Ok("Message not sent!");
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
            var orderInfoVM = JsonConvert.DeserializeObject<OrderInfo>(orderInfo.ToString(), settings);
            await new OrderInfoOperations(_context).Create(orderInfoVM);
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
            var orderProductsVM = JsonConvert.DeserializeObject<List<ProductInOrder>>(orderproducts.ToString(), settings);
            await new ProductInOrderOperations(_context).CreateList(orderProductsVM);
            return Ok();
        }

        [Authorize]
        [HttpGet("ratingdriver/{email}&{orderId}&{rating}")]
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
        [HttpGet("ratingrestaurant/{email}&{orderId}&{rating}")]
        public async Task<IActionResult> GiveRestaurantRating(string email, int orderId, int rating)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();

            var order = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (order == null) return BadRequest();
            var newRating = new RatingCompanie
            {
                Rating = rating,
                OrderRefId = order.OrderId,
                CompanieRefId = order.CompanieRefId,
            };
            order.ClientGaveRatingCompanie = true;
            _context.Orders.Update(order);
            _context.RatingCompanies.Add(newRating);
            await _context.SaveChangesAsync();
            return Ok("Rating acordat");
        }
    }
}
