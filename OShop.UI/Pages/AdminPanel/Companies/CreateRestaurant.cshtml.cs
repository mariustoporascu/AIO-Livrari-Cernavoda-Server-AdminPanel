using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Application.Categories;
using OShop.Application.Companii;
using OShop.Application.FileManager;
using OShop.Application.Products;
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
        public Companie Companie { get; set; }
        [BindProperty]
        public IEnumerable<TipCompanie> TipCompanie { get; set; }

        public IActionResult OnGet(int? restId)
        {
            TipCompanie = _context.TipCompanies.AsNoTracking().AsEnumerable().ToList();
            if (restId == null)
                Companie = new Companie();
            else
            {
                Companie = new GetCompanie(_context).Do(restId);
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
                        if (!string.IsNullOrEmpty(Companie.Photo))
                        {
                            _fileManager.RemoveImage(Companie.Photo, "CompaniePhoto");
                        }
                        Companie.Photo = _fileManager.SaveImage(file, "CompaniePhoto");
                    }
                }
                else if (Request.Form.Files.Count == 0)
                {
                    Companie.Photo = Companie.Photo;
                }


                if (Companie.CompanieId > 0)
                {
                    await new UpdateCompanie(_context).Do(Companie);
                }
                else
                    await new CreateCompanie(_context).Do(Companie);
                return RedirectToPage("./ListaRestaurante");
            }
            return RedirectToPage("/Error", new { Area = "" });
        }
    }
}
