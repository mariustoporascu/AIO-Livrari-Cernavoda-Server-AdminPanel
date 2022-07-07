using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class OrderInfoModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileManager _fileManager;
        private readonly OnlineShopDbContext _context;

        public OrderInfoModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager, IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _fileManager = fileManager;
        }

        [BindProperty]
        public OrderVM Order { get; set; }
        [BindProperty]
        public IEnumerable<ProductVM> Products { get; set; }


        public async Task<IActionResult> OnGet(int vm)
        {
            Order = new OrderOperations(_context, _userManager).GetVM(vm);
            var user = await _userManager.GetUserAsync(User);
            if (!(await _userManager.IsInRoleAsync(user, "SuperAdmin")) && Order.CompanieRefId != user.CompanieRefId)
                return RedirectToPage("/Error");
            Products = new ProductOperations(_context, _fileManager).GetAllVM()
                .Where(prod => Order.ProductsInOrder.Select(product => product.ProductRefId).Contains(prod.ProductId));
            return Page();
        }

    }
}
