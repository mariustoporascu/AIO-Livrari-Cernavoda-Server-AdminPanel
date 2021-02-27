using Microsoft.AspNetCore.Mvc;
using OShop.Application.CartItemsA;
using OShop.Application.Products;
using OShop.Application.ShoppingCarts;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.ReactUI.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly OnlineShopDbContext _context;

        public ShoppingCartController(OnlineShopDbContext context)
        {
            _context = context;
        }


        [HttpGet("getproductincart/{cartId}")]
        public IActionResult GetProductsInCart(int cartId) => Ok(new GetAllProducts(_context).Do(cartId));

        [HttpGet("getcart/{customerId}")]
        public IActionResult GetCart(string customerId) => Ok(new GetShoppingCart(_context).Do(customerId));
               

        [HttpGet("getcartitems/{cartId}")]
        public IActionResult GetCartItems(int cartId) => Ok(new GetCartItems(_context).Do(cartId));

        [HttpPost("addcartitem")]
        public async Task<IActionResult> AddToCart([FromForm] CartItemsViewModel vm,
            [FromForm] decimal Price) {
            if (ModelState.IsValid)
            {
                await new CreateCartItem(_context).Do(vm);
                await new UpdateShoppingCart(_context).UpdateTotal(vm.CartRefId, 1, Price);
                return Ok();
            }
            return BadRequest();
        } 
    }
}
