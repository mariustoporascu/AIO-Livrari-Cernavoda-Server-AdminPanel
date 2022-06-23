using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel
{
    [Authorize(Roles = "SuperAdmin")]
    public class TransportFeesAddModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public TransportFeesAddModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<AvailableCity> Cities { get; set; }
        [BindProperty]
        public List<TransportFee> TransportFees { get; set; }

        public IActionResult OnGet(int tipCompanie, int cityId)
        {
            TransportFees = _context.TransportFees.AsNoTracking().AsEnumerable().Where(tf => tf.MainCityRefId == cityId && tf.TipCompanieRefId == tipCompanie).ToList();
            Cities = _context.AvailableCities.AsNoTracking().AsEnumerable().ToList();
            if (TransportFees.Count == 0)
                foreach (var city in Cities)
                    TransportFees.Add(new TransportFee
                    {
                        CityRefId = city.CityId,
                        TipCompanieRefId = tipCompanie,
                        MainCityRefId = cityId
                    });
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                foreach (var fee in TransportFees)
                {
                    fee.TransporFee = fee.MinimumOrderValue * 15 / 100;
                    if (_context.TransportFees.AsNoTracking().FirstOrDefault(fs => fs.MainCityRefId == fee.MainCityRefId && fs.CityRefId == fee.CityRefId) == null)
                    {
                        _context.TransportFees.Add(fee);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        var feeDB = _context.TransportFees.AsNoTracking().FirstOrDefault(fs => fs.MainCityRefId == fee.MainCityRefId && fs.CityRefId == fee.CityRefId);
                        feeDB.TransporFee = fee.TransporFee;
                        feeDB.MinimumOrderValue = fee.MinimumOrderValue;
                        _context.TransportFees.Update(feeDB);
                        await _context.SaveChangesAsync();

                    }
                }
                return RedirectToPage("/Index");
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
