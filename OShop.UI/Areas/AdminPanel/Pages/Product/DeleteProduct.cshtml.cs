﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Database;
using System.Threading.Tasks;

namespace OShop.UI.Areas.AdminPanel.Pages.Product
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DeleteProductModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteProductModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> OnGet(int canal, int productId)
        {
            await new DeleteProduct(_context, _fileManager).Do(productId);
            return RedirectToPage("./Index", new { canal = canal });
        }
    }
}
