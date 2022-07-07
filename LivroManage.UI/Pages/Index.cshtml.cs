using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LivroManage.Application;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class IndexModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public IEnumerable<ProductVM> Products { get; set; }

        [BindProperty]
        public IEnumerable<Category> Categ { get; set; }

        [BindProperty]
        public int ProduseVandute { get; set; }
        [BindProperty]
        public int TotalCLienti { get; set; }
        [BindProperty]
        public int Comenzi { get; set; }
        [BindProperty]
        public int Rating { get; set; }
        [BindProperty]
        public decimal TotalVanzari { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var startTime = DateTime.UtcNow;
                var user = await _userManager.GetUserAsync(User);
                if (user.CompanieRefId > 0)
                {
                    var orders = await new OrderOperations(_context, _userManager).GetAllVMOwner(user.CompanieRefId);
                    Comenzi = orders.Count();
                    if (Comenzi > 0)
                    {
                        foreach (var order in orders)
                        {
                            var productsInOrder = new ProductInOrderOperations(_context).GetAll(order.OrderId).Select(po => po.UsedQuantity);
                            foreach (var product in productsInOrder)
                                ProduseVandute += product;
                            TotalVanzari += order.TotalOrdered;
                        }
                        TotalCLienti = orders.Select(or => or.CustomerId).Distinct().Count();
                        try
                        {
                            var ratings = _context.RatingCompanies.AsNoTracking().AsEnumerable().Where(rat => rat.CompanieRefId == user.CompanieRefId).Select(ra => ra.Rating);
                            decimal sumRating = ratings.Count() * 5.0M;
                            decimal totalRating = 0.0M;
                            foreach (var rat in ratings)
                                totalRating += rat;
                            Rating = (int)Math.Abs(totalRating / sumRating * 100.0M);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Rating = 0;
                        }

                    }
                    else
                    {
                        TotalCLienti = 0;
                        TotalVanzari = 0;
                        Rating = 0;
                    }

                }
                else
                {
                    Comenzi = 0;
                    TotalCLienti = 0;
                    TotalVanzari = 0;
                    Rating = 0;
                }

                var timeEnd = DateTime.UtcNow;
                var runTime = timeEnd.Subtract(startTime).TotalSeconds;
                Console.WriteLine($"Total time : {runTime}");
                return Page();

            }
            return RedirectToPage("/Auth/Login");

        }

    }
}
