using Microsoft.AspNetCore.Mvc;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using OShop.UI.ApiAuth;
using OShop.UI.Extras;
using System.Threading.Tasks;

namespace OShop.UI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GetDirectionsController : ControllerBase
    {
        private readonly IGoogleApiDirections _googleApiDirections;

        public GetDirectionsController(IGoogleApiDirections googleApiDirections)
        {
            _googleApiDirections = googleApiDirections;
        }
        [HttpGet("getroute/{latCourier}&{longCourier}&{latHome}&{longHome}")]
        public async Task<IActionResult> GetRoutes(double latCourier, double longCourier,double latHome, double longHome )
            => Ok(await _googleApiDirections.GetDirections( latCourier, longCourier,latHome, longHome));
    }
}
