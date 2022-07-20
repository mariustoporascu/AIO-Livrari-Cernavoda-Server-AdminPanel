using Microsoft.AspNetCore.Mvc;
using LivroManage.UI.ApiAuth;
using LivroManage.UI.Google;
using System.Threading.Tasks;

namespace LivroManage.UI.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
        public async Task<IActionResult> GetRoutes(double latCourier, double longCourier, double latHome, double longHome)
            => Ok(await _googleApiDirections.GetDirections(latCourier, longCourier, latHome, longHome));
    }
}
