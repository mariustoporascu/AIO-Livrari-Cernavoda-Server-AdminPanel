using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.SubCategories;
using OShop.Application.FileManager;
using OShop.Application.Restaurante;
using OShop.Application.SuperMarkets;
using OShop.Database;
using System.Collections.Generic;

namespace OShop.UI.Areas.AdminPanel.Pages.SubCategory
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
        public IEnumerable<SubCategoryVMUI> SubCategories { get; set; }

        public int Canal { get; set; }
        public int Category { get; set; }
        public void OnGet(int canal, int category)
        {
            Canal = canal;
            Category = category;
            SubCategories = new GetAllSubCategories(_context, _fileManager).Do(category);
        }
    }
}
