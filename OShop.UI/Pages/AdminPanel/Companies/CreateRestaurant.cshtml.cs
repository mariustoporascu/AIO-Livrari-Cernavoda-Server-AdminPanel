using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OShop.Application.Categories;
using OShop.Application.FileManager;
using OShop.Application.Products;
using OShop.Application.Restaurante;
using OShop.Application.UnitatiMasura;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel.Companies
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateRestaurantModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public CreateRestaurantModel(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }


        [BindProperty]
        public RestaurantVMUI Restaurant { get; set; }

        public IActionResult OnGet( int? restId)
        {

            if (restId == null)
                Restaurant = new RestaurantVMUI();
            else
            {
                Restaurant = new GetRestaurant(_context).Do(restId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var extensionAccepted = new string[] { ".jpg", ".png", ".jpeg" };
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var extension = Path.GetExtension(file.FileName);
                    if (!extensionAccepted.Contains(extension.ToLower()))
                        return RedirectToPage("/Error", new { Area = "" });
                    else
                    {
                        if (!string.IsNullOrEmpty(Restaurant.Photo))
                        {
                            _fileManager.RemoveImage(Restaurant.Photo, "RestaurantPhoto");
                        }
                        Restaurant.Photo = _fileManager.SaveImage(file, "RestaurantPhoto");
                    }
                }
                else if (Request.Form.Files.Count == 0)
                {
                    Restaurant.Photo = Restaurant.Photo;
                }


                if (Restaurant.RestaurantId > 0)
                {
                    await new UpdateRestaurant(_context).Do(Restaurant);
                }
                else
                    await new CreateRestaurant(_context).Do(Restaurant);
                return RedirectToPage("./ListaRestaurante");
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
