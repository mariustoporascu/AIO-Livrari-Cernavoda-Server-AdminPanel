using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.Restaurante;
using OShop.Application.SubCategories;
using OShop.Application.SuperMarkets;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace OShop.UI.Areas.AdminPanel.Pages.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IndexModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public IndexModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        [BindProperty]
        public IEnumerable<ProductVMUI> Products { get; set; }

        [BindProperty]
        public IEnumerable<CategoryVMUI> Categ { get; set; }
        [BindProperty]
        public IEnumerable<SubCategoryVMUI> SubCateg { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        [BindProperty]
        public IEnumerable<SuperMarketVMUI> SuperMarkets { get; set; }
        [BindProperty]
        public IEnumerable<RestaurantVMUI> Restaurante { get; set; }
        [BindProperty]
        public IEnumerable<UnitateMasuraVMUI> UnitatiMasura { get; set; }
        public void OnGet(int canal)
        {
            Canal = canal;
            Products = new GetAllProducts(_context, _fileManager).Do(canal);
            Categ = new GetAllCategories(_context, _fileManager).Do(canal);
            SuperMarkets = new GetAllSuperMarkets(_context, _fileManager).Do();
            Restaurante = new GetAllRestaurante(_context, _fileManager).Do();
            UnitatiMasura = new GetAllMeasuringUnits(_context).Do();
            SubCateg = new GetAllSubCategories(_context, _fileManager).Do();
        }
    }
}
