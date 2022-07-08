using System.Threading.Tasks;

namespace LivroManage.UI.Google
{
    public interface IGoogleApiDirections
    {
        Task<string> GetDirections(double latCourier, double longCourier, double latHome, double longHome);
    }
}
