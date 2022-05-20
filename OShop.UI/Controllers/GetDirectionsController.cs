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
        [HttpGet("getroute/{latHome}&{longHome}&{latCourier}&{longCourier}")]
        public async Task<IActionResult> GetRoutes(double latHome, double longHome, double latCourier, double longCourier)
            => Ok(await _googleApiDirections.GetDirections(latHome, longHome, latCourier, longCourier));
    }
}
