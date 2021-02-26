using Microsoft.AspNetCore.Mvc;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using System.Threading.Tasks;

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
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context).Do(0));

        [HttpGet("getproduct/{productId}")]
        public IActionResult GetProduct(int productId) => Ok();

        [HttpPost("createproduct")]
        public async Task<IActionResult> AddProductAsync([FromForm] ProductVMReactUI vm)
        {
            if (ModelState.IsValid)
            {
                await new CreateProduct(_context, _fileManager).DoReact(vm);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProductAsync([FromForm] ProductVMReactUI vm)
        {
            if (ModelState.IsValid)
            {
                await new UpdateProduct(_context, _fileManager).DoReact(vm);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("deleteproduct/{name}")]
        public async Task<IActionResult> RemoveProductAsync(string name)
        {
            await new DeleteProduct(_context, _fileManager).Do(name);
            return Ok();
        }

        [HttpGet("getallcategories")]
        public IActionResult ManageCategories() => Ok(new GetAllCategories(_context).Do());

        [HttpPost("createcategory")]
        public async Task<IActionResult> AddCategoryAsync([FromForm] CategoryVMReactUI vm)
        {
            if (ModelState.IsValid)
            {
                await new CreateCategory(_context, _fileManager).DoReact(vm);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("updatecategory")]
        public async Task<IActionResult> UpdateCategoryAsync([FromForm] CategoryVMReactUI vm)
        {
            if (ModelState.IsValid)
            {
                await new UpdateCategory(_context, _fileManager).DoReact(vm);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("deletecategory/{name}")]
        public async Task<IActionResult> RemoveCategoryAsync(string name)
        {
            await new DeleteCategory(_context, _fileManager).Do(name);
            return Ok();
        }
    }
}
