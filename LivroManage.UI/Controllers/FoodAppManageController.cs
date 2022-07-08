using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using LivroManage.Application;
using LivroManage.Database;
using LivroManage.Domain.Models;
using LivroManage.UI.ApiAuthManage;
using LivroManage.UI.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FoodAppManageController : ControllerBase
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int maxAllowedOrdersForDriver;
        private readonly string OneSignalApiKey;
        private readonly string OneSignalAppId;


        public FoodAppManageController(IConfiguration config, OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            OneSignalApiKey = config["ConnectionStrings:SignalApiKey"];
            OneSignalAppId = config["ConnectionStrings:SignalAppId"];
            maxAllowedOrdersForDriver = int.Parse(config["ConnectionStrings:MaxNrOrdersDriver"]);
        }
        [Authorize]
        [HttpGet("updatetelno/{id}&{telno}")]
        public async Task<IActionResult> UpdateTelNo(string id, string telno)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && (user.IsDriver || user.IsOwner))
            {
                user.PhoneNumber = telno;
                await _userManager.UpdateAsync(user);
                return Ok("Phone nr updated.");
            }
            return Ok("Data invalid.");
        }
        [Authorize]
        [HttpGet("getalldriverorders")]
        public IActionResult GetAllOrders() =>
            Ok(new OrderOperations(_context, _userManager).GetAllVMDriver());
        [Authorize]
        [HttpGet("getallrestaurantorders/{companyRefId}")]
        public async Task<IActionResult> GetAllOrders(int companyRefId) =>
            Ok(await new OrderOperations(_context, _userManager).GetAllVMOwner(companyRefId));
        [Authorize]
        [HttpGet("updatestatus/{orderId}&{status}&{isOwner}")]
        public async Task<IActionResult> OrderStatus(int orderId, string status, bool isOwner)
        {
            var notifSender = new NotificationSender();
            if (await new OrderOperations(_context, _userManager).UpdateBool(orderId, status))
            {
                if (status == "In pregatire")
                {
                    var drivers = _userManager.Users.Where(us => us.IsDriver == true).ToList();
                    foreach (var driver in drivers)
                    {
                        var driverToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == driver.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                        if (driverToken != null)
                        {
                            foreach (var token in driverToken)
                                await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"A aparut o noua comanda fara livrator!");
                        }
                    }
                }
                var orderVM = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);

                if (!isOwner && !status.Contains("Livrata") && !status.Contains("Refuzata") && !status.Contains("In curs de livrare"))
                {
                    var restaurant = _userManager.Users.FirstOrDefault(us => us.CompanieRefId == orderVM.CompanieRefId);
                    if (restaurant != null)
                    {
                        var restaurantToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == restaurant.Id)
                            .Select(tkn => tkn.FBToken).Distinct().ToList();
                        if (restaurantToken != null)
                        {
                            foreach (var token in restaurantToken)
                                await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"Statusul comenzii {orderId} a fost schimbat in {status}!");
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(orderVM.DriverRefId))
                {
                    var driverToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == orderVM.DriverRefId)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (driverToken != null)
                    {
                        foreach (var token in driverToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"Statusul comenzii {orderId} a fost schimbat in {status}!");
                    }
                }
                if (!orderVM.TelephoneOrdered)
                {
                    var userToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == orderVM.CustomerId)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (userToken != null)
                    {
                        foreach (var token in userToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, status.Contains("In curs de livrare") ? $"Comanda {orderId} este in curs de livrare catre tine, o poti urmari pe harta."
                                : $"Statusul comenzii {orderId} a fost schimbat in {status}!");
                    }
                }

                return Ok("Order status updated.");

            }
            return Ok("Order not found!");
        }
        [Authorize]
        [HttpGet("adjustOrder/{orderId}&{comment}&{newTotal}")]
        public async Task<IActionResult> AdjustProducts(int orderId, string comment, decimal newTotal)
        {

            var order = _context.Orders.AsNoTracking().AsEnumerable().FirstOrDefault(ord => ord.OrderId == orderId);
            if (order != null)
            {
                order.Comments = comment;
                order.TotalOrdered = newTotal;

                if (!order.TelephoneOrdered)
                {
                    var locationCity = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.LocationId == order.UserLocationId).City;
                    var city = _context.AvailableCities.AsNoTracking().FirstOrDefault(ct => ct.Name == locationCity);
                    var transportFee = _context.TransportFees.AsNoTracking().FirstOrDefault(tf => tf.CityRefId == city.CityId);
                    if (order.TotalOrdered >= transportFee.MinimumOrderValue)
                        order.TransportFee = 0;
                    else
                        order.TransportFee = transportFee.TransporFee;
                    var userToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == order.CustomerId)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (userToken != null)
                    {
                        var notifSender = new NotificationSender();
                        foreach (var token in userToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"Comanda {orderId} a fost modificata conform cererii tale, are un nou pret total si un comentariu.");
                    }
                }
                else
                {
                    var locationCity = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == $"{order.OrderId}").City;
                    var city = _context.AvailableCities.AsNoTracking().FirstOrDefault(ct => ct.Name == locationCity);
                    var transportFee = _context.TransportFees.AsNoTracking().FirstOrDefault(tf => tf.CityRefId == city.CityId);
                    if (order.TotalOrdered >= transportFee.MinimumOrderValue)
                        order.TransportFee = 0;
                    else
                        order.TransportFee = transportFee.TransporFee;
                }
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return Ok("Comanda a fost modificata");
            }
            return Ok("Comanda nu a fost modificata");
        }
        [Authorize]
        [HttpGet("driverlockorder/{email}&{orderId}")]
        public async Task<IActionResult> DriverLockorder(string email, int orderId)
        {
            var notifSender = new NotificationSender();
            var driverId = (await _userManager.FindByEmailAsync(email)).Id;
            if (_context.Orders.AsNoTracking().Where(ord => ord.DriverRefId == driverId && ord.Status != "Refuzata" && ord.Status != "Livrata").Count() >= maxAllowedOrdersForDriver)
            {
                return Ok("Ai atins maximum de comenzi care pot fi luate.");
            }
            if (await new OrderOperations(_context, _userManager).LockByDriver(driverId, orderId))
            {
                var orderVM = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
                var restaurant = _userManager.Users.FirstOrDefault(us => us.CompanieRefId == orderVM.CompanieRefId);
                if (restaurant != null)
                {
                    var restaurantToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == restaurant.Id)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (restaurantToken != null)
                    {
                        foreach (var token in restaurantToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"La comanda {orderId} s-a alaturat un livrator pentru livrarea ulterioara!");
                    }
                }
                if (!orderVM.TelephoneOrdered)
                {
                    var userToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == orderVM.CustomerId)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                    if (userToken != null)
                    {
                        foreach (var token in userToken)
                            await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"La comanda {orderId} s-a alaturat un livrator pentru livrarea ulterioara!");
                    }
                }

                return Ok("Order locked.");
            }

            return Ok("Order not locked.");
        }
        [Authorize]
        [HttpPost("driverupdatelocation")]
        public async Task<IActionResult> DriverUpdateLocation([FromBody] object driverLocation)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var location = JsonConvert.DeserializeObject<UserLocations>(driverLocation.ToString(), settings);
            var user = await _userManager.FindByIdAsync(location.UserId);
            if (user != null)
            {
                var locationDb = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == user.Id);
                if (locationDb == null || locationDb.LocationId == 0)
                {
                    location.UserId = user.Id;
                    location.LocationName = user.FullName;
                    _context.UserLocations.Add(location);
                }

                else
                {
                    location.LocationId = locationDb.LocationId;
                    location.UserId = user.Id;
                    location.LocationName = user.FullName;

                    _context.UserLocations.Update(location);
                }
                await _context.SaveChangesAsync();
                return Ok("Location updated");
            }
            return Ok("Location not updated");
        }
        [Authorize]
        [HttpGet("setesttime/{orderId}&{esttime}")]
        public async Task<IActionResult> SetEstTime(int orderId, string esttime)
        {
            var orderVM = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (!orderVM.TelephoneOrdered)
            {
                var userToken = _context.FBTokens.AsNoTracking().Where(tkn => tkn.UserId == orderVM.CustomerId)
                        .Select(tkn => tkn.FBToken).Distinct().ToList();
                if (userToken != null)
                {
                    var notifSender = new NotificationSender();
                    foreach (var token in userToken)
                        await notifSender.SendNotif(OneSignalApiKey, OneSignalAppId, token, $"Ai primit un timp estimat de pregatire al comenzii cu numarul {orderId}, te rugam sa iti exprimi acordul!");
                }
            }

            return Ok($"estTime : {await new OrderOperations(_context, _userManager).SetEstTime(orderId, esttime)}");
        }
        [Authorize]
        [HttpGet("toggleordering/{companieId}")]
        public async Task<IActionResult> ToggleOrdering(int companieId)
        {
            var companie = _context.Companies.AsNoTracking().FirstOrDefault(comp => comp.CompanieId == companieId);
            if (companie != null)
            {
                companie.TemporaryClosed = !companie.TemporaryClosed;
                _context.Companies.Update(companie);
                await _context.SaveChangesAsync();
                return Ok("Campul a fost modificat");

            }
            return Ok("Data invalid.");
        }
        [Authorize]
        [HttpGet("toggleproduct/{companieId}&{productId}")]
        public async Task<IActionResult> ToggleOrdering(int companieId, int productId)
        {
            var companie = _context.Companies.AsNoTracking().FirstOrDefault(comp => comp.CompanieId == companieId);
            if (companie != null)
            {
                var product = _context.Products.AsNoTracking().FirstOrDefault(prod => prod.ProductId == productId);
                if (product != null)
                {
                    product.IsAvailable = !product.IsAvailable;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                return Ok("Campul a fost modificat");

            }
            return Ok("Data invalid.");
        }
        [Authorize]
        [HttpGet("ratingclient/{isOwner}&{orderId}&{rating}")]
        public async Task<IActionResult> GiveRestaurantRating(bool isOwner, int orderId, int rating)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(or => or.OrderId == orderId);
            if (order == null) return BadRequest();
            var haveRating = _context.RatingClients.AsNoTracking().FirstOrDefault(rc => rc.OrderRefId == orderId);
            if (haveRating != null)
            {
                if (isOwner)
                    haveRating.RatingDeLaCompanie = rating;
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
                    haveRating.RatingDeLaCompanie = rating;
                else
                    haveRating.RatingDeLaSofer = rating;
                _context.RatingClients.Add(haveRating);

            }

            if (isOwner)
            {
                order.CompanieGaveRating = true;
            }
            else
            {
                order.DriverGaveRating = true;
            }
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return Ok("Rating acordat");
        }
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("getmyearnings")]
        public async Task<IActionResult> FetchTotalComenzi()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();
            var orders = await new OrderOperations(_context, _userManager).GetAllVMOwner(user.CompanieRefId);
            decimal[] valori = new decimal[12];
            orders = orders.OrderByDescending(o => o.Created);

            for (int i = 0; i < 12; i++)
            {
                var monthOrders = orders.Where(or => or.Created.Month == i + 1).ToList();
                decimal totalLuna = 0.0M;
                foreach (var order in monthOrders)
                    totalLuna += order.TotalOrdered;
                valori[i] = totalLuna;
            }
            return Ok(JsonConvert.SerializeObject(valori));
        }

    }
}
