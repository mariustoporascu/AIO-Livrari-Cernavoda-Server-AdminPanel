using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OShop.Database;
using System.Threading.Tasks;
using OShop.Application.Products;
using OShop.Application.Categories;
using OShop.Application.FileManager;

namespace OShop.ReactUI.Controllers
{

    [Route("[controller]")]
    public class AdminPanelController : Controller
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public AdminPanelController(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [HttpGet("getallproducts")]
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context).Do());

        [HttpGet("getproduct/{productId}")]
        public IActionResult GetProduct(int productId) => Ok();
        
        [HttpPost("createproduct")]
        public async Task<IActionResult> AddProductAsync([FromForm] ProductVMReactUI vm) {
            if (ModelState.IsValid)
            {
                await new CreateProduct(_context, _fileManager).DoReact(vm);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProductAsync([FromForm] ProductVMReactUI vm) {
            if (ModelState.IsValid)
            {
                await new UpdateProduct(_context, _fileManager).DoReact(vm);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("deleteproduct/{productId}")]
        public async Task<IActionResult> RemoveProductAsync(int productId)
        {
            await new DeleteProduct(_context, _fileManager).Do(productId);
            return Ok();
        }
        
        [HttpGet("getallcategories")]
        public IActionResult ManageCategories() => Ok(new GetAllCategories(_context).Do());


    }
}
