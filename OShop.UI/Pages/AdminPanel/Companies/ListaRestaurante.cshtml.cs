using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.Companii;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Companies
{
    [Authorize(Roles = "SuperAdmin")]
    public class ListaRestauranteModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public ListaRestauranteModel(OnlineShopDbContext context)
        {
            _context = context;
        }


        [BindProperty]
        public IEnumerable<CompanieVMUI> Restaurante { get; set; }
        public IActionResult OnGet()
        {

            Restaurante = new GetAllCompanii(_context).Do();

            return Page();
        }
    }
}
