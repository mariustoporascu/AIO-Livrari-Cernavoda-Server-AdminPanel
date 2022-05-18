using Microsoft.AspNetCore.Mvc;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;

namespace OShop.UI.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeController(OnlineShopDbContext context,IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        [HttpGet("getallproducts")]
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context, _fileManager).Do(0,0));
    }
}
