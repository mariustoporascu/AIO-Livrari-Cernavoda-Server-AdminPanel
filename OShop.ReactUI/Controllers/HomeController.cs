using Microsoft.AspNetCore.Mvc;
using OShop.Application.Products;
using OShop.Database;

namespace OShop.ReactUI.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly OnlineShopDbContext _context;


        public HomeController(OnlineShopDbContext context)
        {
            _context = context;

        }
        [HttpGet("getallproducts")]
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context).Do(0));
    }
}
