using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public  class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileManager _fileManager;
        private readonly OnlineShopDbContext _context;


        public ProfileModel(
            OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _fileManager = fileManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string UserNameChangeLimitMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Numar de telefon")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Poza restaurant")]
            public string ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            try
            {
                var restaurant = _context.Restaurante.AsNoTracking().FirstOrDefault(rest => rest.RestaurantId == user.RestaurantRefId);
                if(restaurant != null)
                    Input = new InputModel
                    {
                        PhoneNumber = restaurant.TelefonNo,
                        ProfilePicture = restaurant.Photo
                    };
                else
                {
                    var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                    Input = new InputModel
                    {
                        PhoneNumber = phoneNumber,
                        ProfilePicture = user.ProfilePicture
                    };
                }
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s).";
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var restaurant = _context.Restaurante.AsNoTracking().FirstOrDefault(rest => rest.RestaurantId == user.RestaurantRefId);
            
            if (Request.Form.Files.Count > 0)
            {
                var extensionAccepted = new string[] { ".jpg", ".png", ".jpeg" };
                IFormFile file = Request.Form.Files.FirstOrDefault();
                var extension = Path.GetExtension(file.FileName);
                if (!extensionAccepted.Contains(extension.ToLower()))
                    return RedirectToPage("/Error", new { Area = "" });
                else
                {
                    if (!string.IsNullOrEmpty(user.ProfilePicture))
                    {
                        _fileManager.RemoveImage(user.ProfilePicture, "ProfilePhoto");
                    }
                    user.ProfilePicture = _fileManager.SaveImage(file, "ProfilePhoto");
                    await _userManager.UpdateAsync(user);
                    if(restaurant != null)
                    {
                        if (!string.IsNullOrEmpty(restaurant.Photo))
                        {
                            _fileManager.RemoveImage(restaurant.Photo, "RestaurantPhoto");
                        }
                        restaurant.Photo = _fileManager.SaveImage(file, "RestaurantPhoto");
                        _context.Restaurante.Update(restaurant);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profil actualizat";
            return RedirectToPage();
        }
    }
}
