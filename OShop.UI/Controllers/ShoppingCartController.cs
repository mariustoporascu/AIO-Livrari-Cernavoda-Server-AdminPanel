using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OShop.Application.CartItemsA;
using OShop.Application.FileManager;
using OShop.Application.OrderInfos;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.UI.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartController(OnlineShopDbContext context,
            IFileManager fileManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _fileManager = fileManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("getcart/{customerId}")]
        public async Task<IActionResult> AnonymousCart(string customerId)
        {
            var currUser = customerId;
            var cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];

            if (cookieValueFromContext != null)
            {
                var cookieCart = new GetShoppingCart(_context).Do(cookieValueFromContext);
                if (currUser != "undefined" && cookieCart != null)
                {
                    var userCart = new GetShoppingCart(_context).Do(currUser);
                    if (cookieCart.CustomerId != currUser && userCart == null)
                    {
                        cookieCart.CustomerId = currUser;
                        await new UpdateShoppingCart(_context).Do(cookieCart);
                        _httpContextAccessor.HttpContext.Response.Cookies.Delete("anonymousUsr");
                        return Ok(cookieCart);
                    }
                    else
                        return Ok(userCart);
                }
                else if (cookieCart == null)
                {
                    await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
                    {
                        CustomerId = cookieValueFromContext,
                    });
                    return Ok(new GetShoppingCart(_context).Do(cookieValueFromContext));
                }
                else
                    return Ok(cookieCart);
            }
            else if (currUser != "undefined")
            {
                var usercart = new GetShoppingCart(_context).Do(currUser);
                if (usercart == null)
                {
                    await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
                    {
                        CustomerId = currUser,
                    });
                    return Ok(new GetShoppingCart(_context).Do(currUser));
                }
                return Ok(usercart);
            }

            else
            {
                var userId = Guid.NewGuid().ToString();
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10);
                option.SameSite = SameSiteMode.Strict;
                option.Secure = false;
                option.HttpOnly = true;
                Response.Cookies.Append("anonymousUsr", userId, option);
                await new CreateShoppingCart(_context).Do(new ShoppingCartViewModel
                {
                    CustomerId = userId,
                });
                return Ok(new GetShoppingCart(_context).Do(userId));
            }
        }

        [HttpGet("getproductincart/{cartId}")]
        public IActionResult GetProductsInCart(int cartId) => Ok(new GetAllProducts(_context,_fileManager).Do(cartId,0));


        [HttpGet("getcartitems/{cartId}")]
        public IActionResult GetCartItems(int cartId) => Ok(new GetCartItems(_context).Do(cartId));

        [HttpPost("addcartitem")]
        public async Task<IActionResult> AddToCart([FromForm] CartItemsViewModel vm,
            [FromForm] decimal Price)
        {
            if (ModelState.IsValid)
            {
                await new CreateCartItem(_context).Do(vm);
                await new UpdateShoppingCart(_context).UpdateTotal(vm.CartRefId, 1, Price);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("updatecartitem")]
        public async Task<IActionResult> UpdateCartItem([FromForm] CartItemsViewModel vm,
            [FromForm] decimal Price, [FromForm] int PrevQuantity)
        {
            if (ModelState.IsValid)
            {
                await new UpdateCartItem(_context).Do(new CartItemsViewModel
                {
                    CartRefId = vm.CartRefId,
                    ProductRefId = vm.ProductRefId,
                    Quantity = vm.Quantity,
                });
                if (PrevQuantity > vm.Quantity)
                    await new UpdateShoppingCart(_context).UpdateTotal(vm.CartRefId, PrevQuantity - vm.Quantity, -Price);
                else
                    await new UpdateShoppingCart(_context).UpdateTotal(vm.CartRefId, vm.Quantity - PrevQuantity, Price);

                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("removecartitem")]
        public async Task<IActionResult> RemoveCartItem(int CartRefId, int ProductRefId,
            int Quantity, decimal Price)
        {
            if (ModelState.IsValid)
            {
                await new DeleteCartItem(_context).Do(CartRefId, ProductRefId);
                await new UpdateShoppingCart(_context).UpdateTotal(CartRefId, Quantity, -Price);
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("orderinfo/{customerId}")]
        public async Task<IActionResult> GetOrderInfo(string customerId)
        {
            var currUser = customerId;
            if (currUser == "undefined")
            {
                currUser = _httpContextAccessor.HttpContext.Request.Cookies["anonymousUsr"];
            }
            var Order = new GetOrder(_context).Do(currUser, "Pending");
            if (Order == null)
            {
                await new CreateOrder(_context).Do(new OrderViewModel
                {
                    Status = "Pending",
                    CustomerId = currUser
                });
                Order = new GetOrder(_context).Do(currUser, "Pending");
            }
            var OrderInfos = new GetOrderInfo(_context).Do(Order.OrderId);
            if (OrderInfos == null)
                OrderInfos = new OrderInfosViewModel { OrderRefId = Order.OrderId };
            return Ok(OrderInfos);
        }

        [HttpPost("addorderinfo")]
        public async Task<IActionResult> AddOrderInfo([FromForm] OrderInfosViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await new CreateOrderInfo(_context).Do(vm);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("updateorderinfo")]
        public async Task<IActionResult> UpdateOrderInfo([FromForm] OrderInfosViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await new UpdateOrderInfo(_context).Do(vm);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("placeorder")]
        public async Task<IActionResult> PlaceOrder([FromForm] OrderViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await new UpdateOrder(_context).Do(vm);
                var ShoppingCart = new GetShoppingCart(_context).Do(vm.CustomerId);
                var CartItems = new GetCartItems(_context).Do(ShoppingCart.CartId);
                var Products = new GetAllProducts(_context,_fileManager).Do(ShoppingCart.CartId,0)
                    .Where(prod => CartItems.Select(cartItem => cartItem.ProductRefId)
                    .Contains(prod.ProductId));

                foreach (var cartitem in CartItems.ToList())
                {
                    await new CreateProductInOrder(_context).Do(new ProductInOrdersViewModel
                    {
                        OrderRefId = vm.OrderId,
                        ProductRefId = cartitem.ProductRefId,
                        UsedQuantity = cartitem.Quantity,
                    });
                    await new UpdateProduct(_context, _fileManager).UpdateStockAfterOrder(cartitem.ProductRefId, cartitem.Quantity);
                }
                ShoppingCart.Status = "Closed";
                await new UpdateShoppingCart(_context).Do(ShoppingCart);
                return Ok();
            }
            return BadRequest();
        }
    }
}
