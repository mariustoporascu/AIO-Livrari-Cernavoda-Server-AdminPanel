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
        [BindProperty]
        public int TipCompanie { get; set; }
        public IActionResult OnGet(int tipCompanie)
        {
            Cities = _context.AvailableCities.AsNoTracking().AsEnumerable().ToList();
            TransportFees = new List<TransportFee>();
            foreach (var city in Cities)
                TransportFees.Add(new TransportFee
                {
                    CityRefId = city.CityId,
                });
            TipCompanie = tipCompanie;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var allCompanies = _context.Companies.AsNoTracking().Where(comp => comp.TipCompanieRefId == TipCompanie).AsEnumerable().ToList();
                foreach (var fee in TransportFees)
                {
                    foreach (var company in allCompanies)
                    {
                        if (_context.TransportFees.AsNoTracking().FirstOrDefault(fs => fs.CompanieRefId == company.CompanieId && fs.CityRefId == fee.CityRefId) == null)
                        {
                            fee.CompanieRefId = company.CompanieId;
                            _context.TransportFees.Add(fee);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            var feeDB = _context.TransportFees.AsNoTracking().FirstOrDefault(fs => fs.CompanieRefId == company.CompanieId && fs.CityRefId == fee.CityRefId);
                            feeDB.TransporFee = fee.TransporFee;
                            feeDB.MinimumOrderValue = fee.MinimumOrderValue;
                            _context.TransportFees.Update(feeDB);
                            await _context.SaveChangesAsync();

                        }
                    }
                }
                return RedirectToPage("/Index");
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
