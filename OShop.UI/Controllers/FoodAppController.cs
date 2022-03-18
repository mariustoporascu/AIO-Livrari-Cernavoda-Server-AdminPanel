using Microsoft.AspNetCore.Mvc;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.OrderInfos;
using OShop.Application.Orders;
using OShop.Application.ProductInOrders;
using OShop.Application.Products;
using OShop.Application.Restaurante;
using OShop.Application.SuperMarkets;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using System.Threading.Tasks;

namespace OShop.UI.Controllers
{

    [Route("[controller]")]
    public class FoodAppController : Controller
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public FoodAppController(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [HttpGet("getallproducts")]
        public IActionResult ManageProducts() => Ok(new GetAllProducts(_context, _fileManager).Do());


        [HttpGet("getproduct/{productId}")]
        public IActionResult GetProduct(int productId) => Ok();

        /*[HttpPost("createproduct")]
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

        [HttpDelete("deleteproduct/{productId}")]
        public async Task<IActionResult> RemoveProductAsync(int productId)
        {
            await new DeleteProduct(_context, _fileManager).Do(productId);
            return Ok();
        }*/

        [HttpGet("getallcategories")]
        public IActionResult ManageCategories() => Ok(new GetAllCategories(_context, _fileManager).Do());

        /*        [HttpPost("createcategory")]
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

                [HttpDelete("deletecategory/{categoryId}")]
                public async Task<IActionResult> RemoveCategoryAsync(int categoryId)
                {
                    await new DeleteCategory(_context, _fileManager).Do(categoryId);
                    return Ok();
                }*/

        [HttpGet("getallorders")]
        public IActionResult GetAllOrders() => Ok(new GetAllOrders(_context).Do());

        [HttpGet("getorderinfo/{orderId}")]
        public IActionResult GetOrderInfo(int orderId)
            => Ok(new GetOrderInfo(_context).Do(orderId));

        [HttpGet("getproductsinorder/{orderId}")]
        public IActionResult GetProductsInOrder(int orderId)
            => Ok(new GetAllProductInOrder(_context).Do(orderId));

        [HttpGet("getproductsfororder/{orderId}")]
        public IActionResult GetProductsForOrder(int orderId) => Ok(new GetAllProducts(_context, _fileManager).Do(0, orderId));
        [HttpGet("getallrestaurante")]
        public IActionResult ManageRestaurante() => Ok(new GetAllRestaurante(_context, _fileManager).Do());
        [HttpGet("getallsupermarkets")]
        public IActionResult ManageSuperMarkets() => Ok(new GetAllSuperMarkets(_context, _fileManager).Do());

        [HttpGet("getallmeasuringunits")]
        public IActionResult ManageMeasuringUnits() => Ok(new GetAllMeasuringUnits(_context).Do());
    }
}
