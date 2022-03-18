using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Restaurante;
using OShop.Application.SuperMarkets;
using OShop.Database;
using System.Collections.Generic;

namespace OShop.UI.Areas.AdminPanel.Pages.Category
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
        public IEnumerable<CategoryVMUI> Categories { get; set; }
        [BindProperty]
        public IEnumerable<SuperMarketVMUI> SuperMarkets { get; set; }
        [BindProperty]
        public IEnumerable<RestaurantVMUI> Restaurante { get; set; }
        [BindProperty]
        public int Canal { get; set; }
        public void OnGet(int canal)
        {
            Canal = canal;
            Categories = new GetAllCategories(_context, _fileManager).Do(canal);
            SuperMarkets = new GetAllSuperMarkets(_context, _fileManager).Do();
            Restaurante = new GetAllRestaurante(_context, _fileManager).Do();
        }
    }
}
