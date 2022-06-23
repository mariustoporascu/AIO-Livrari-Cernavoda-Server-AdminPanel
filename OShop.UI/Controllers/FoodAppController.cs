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
using OShop.Application.Companii;
using OShop.Application.SubCategories;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using OShop.UI.ApiAuth;
using OShop.UI.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OShop.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodAppController : ControllerBase
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string OneSignalApiKey;
        private readonly string OneSignalAppId;
        private readonly string FBLoginIosEnabled;
        private readonly IConfiguration _config;
        public FoodAppController(IConfiguration config, OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            OneSignalApiKey = config["ConnectionStrings:SignalApiKey"];
            OneSignalAppId = config["ConnectionStrings:SignalAppId"];
            FBLoginIosEnabled = config["ConnectionStrings:FBLoginIosEnabled"];
        }

        [HttpGet("getallproducts")]
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context).Do());

        [HttpGet("getallcategories")]
        public IActionResult ManageCategories() => Ok(new GetAllCategories(_context).Do());
        [HttpGet("getallcities")]
        public IActionResult ManageCities() => Ok(_context.AvailableCities.AsNoTracking().AsEnumerable().Where(city => city.IsAvailable == true));

        [HttpGet("getallsubcategories")]
        public IActionResult ManageSubCategories() => Ok(new GetAllSubCategories(_context).Do());
        [HttpGet("fbbtnios")]
        public IActionResult FbIos() => Ok(FBLoginIosEnabled);

        [HttpGet("getallcompanii")]
        public IActionResult ManageRestaurante() => Ok(new GetAllCompanii(_context).Do());
        [HttpGet("getalltipcompanii")]
        public IActionResult ManageTipCompanii() => Ok(_context.TipCompanies.AsNoTracking().AsEnumerable());

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
        [HttpGet("agreeesttime/{orderId}&{accept}")]
        public async Task<IActionResult> SetEstTime(int orderId, bool accept)
        {
            var orderVM = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (!orderVM.TelephoneOrdered)
            {
                var restaurant = _userManager.Users.FirstOrDefault(us => us.CompanieRefId == orderVM.CompanieRefId);
                if (restaurant != null)
                {
                    var restaurantToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == restaurant.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (restaurantToken != null)
                    {
                        foreach (var token in restaurantToken)
                            NotificationSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, accept ? "Timpul estimat a fost acceptat!" : "Timpul estimat nu a fost acceptat. Poti anula comanda!");
                    }

                }
            }


            return Ok($"agreed : {await new UpdateOrder(_context).DoET(orderId, accept)}");
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
        partial class ServerOrder
        {
            public int OrderId { get; set; }
            public string Status { get; set; }
            public decimal TotalOrdered { get; set; }
            public decimal TransportFee { get; set; }
            public string PaymentMethod { get; set; }
            public bool IsOrderPayed { get; set; }
            public string CustomerId { get; set; }
            public bool TelephoneOrdered { get; set; }
            public string DriverRefId { get; set; }
            public int CompanieRefId { get; set; }
            public string EstimatedTime { get; set; }
            public bool ClientGaveRatingDriver { get; set; } = false;
            public bool ClientGaveRatingCompanie { get; set; } = false;
            public bool? HasUserConfirmedET { get; set; }
            //public int RatingClient { get; set; }
            public int RatingDriver { get; set; }
            public int RatingCompanie { get; set; }
            public int UserLocationId { get; set; }
            [JsonProperty("orderLocation")]
            public UserLocations UserLocation { get; set; }
            public DateTime Created { get; set; }
            [JsonProperty("productsInOrder")]
            public List<ProductInOrder> ProductsInOrder { get; set; }
            [JsonProperty("orderInfo")]
            public OrderInfo OrderInfos { get; set; }

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

                var orderVM = new Order
                {
                    OrderId = 0,
                    TotalOrdered = order.TotalOrdered,
                    TransportFee = order.TransportFee,
                    CompanieRefId = order.CompanieRefId,
                    PaymentMethod = order.PaymentMethod,
                    IsOrderPayed = order.IsOrderPayed,
                    UserLocationId = order.UserLocationId,
                    TelephoneOrdered = order.TelephoneOrdered,
                    Created = order.Created,
                };

                if (!order.TelephoneOrdered)
                {
                    orderVM.CustomerId = user.Id;
                    orderVM.Status = OrderStatusEnum.Plasata;
                }
                else
                {
                    orderVM.EstimatedTime = order.EstimatedTime;
                    orderVM.HasUserConfirmedET = true;
                    orderVM.Status = OrderStatusEnum.Pregatire;
                    var drivers = _userManager.Users.Where(us => us.IsDriver == true).ToList();
                    foreach (var driver in drivers)
                    {
                        var driverToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == driver.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                        if (driverToken != null)
                        {
                            foreach (var token in driverToken)
                                NotificationSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"A aparut o noua comanda fara livrator!");
                        }
                    }
                }
                int orderId = await new CreateOrder(_context).Do(orderVM);
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
                order.OrderInfos.OrderRefId = orderId;
                await new CreateOrderInfo(_context).Do(order.OrderInfos);
                foreach (var product in order.ProductsInOrder)
                    product.OrderRefId = orderId;
                await new CreateProductInOrder(_context).Do(order.ProductsInOrder);
                var restaurant = _userManager.Users.FirstOrDefault(us => us.CompanieRefId == orderVM.CompanieRefId);
                if (restaurant != null && !order.TelephoneOrdered)
                {
                    var restaurantToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == restaurant.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (restaurantToken != null)
                    {
                        foreach (var token in restaurantToken)
                            NotificationSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"Ai primit o noua comanda in sistem cu numarul {orderId}!");
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
            var orderProductsVM = JsonConvert.DeserializeObject<List<ProductInOrder>>(orderproducts.ToString(), settings);
            await new CreateProductInOrder(_context).Do(orderProductsVM);
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
