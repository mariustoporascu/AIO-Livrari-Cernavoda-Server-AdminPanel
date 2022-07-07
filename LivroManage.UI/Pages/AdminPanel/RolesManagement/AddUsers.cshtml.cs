using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LivroManage.Application;
using LivroManage.Application.FileManager;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LivroManage.UI.Pages.AdminPanel.RolesManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class AddUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public AddUsersModel(OnlineShopDbContext context,
            UserManager<ApplicationUser> userManager,
            IFileManager fileManager)
        {
            _userManager = userManager;
            _context = context;
            _fileManager = fileManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        [BindProperty]
        public List<CompanieVM> Companies { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Prenume")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Nume")]
            public string LastName { get; set; }
            [Display(Name = "Este Companie?")]
            public bool IsOwner { get; set; }
            [Display(Name = "Daca da care?")]
            public int CompanieRefId { get; set; }

            [Display(Name = "Este sofer?")]
            public bool IsDriver { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} trebuie sa aiba minim {2} si maxim {1} caractere", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Parola")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirma parola")]
            [Compare("Password", ErrorMessage = "Parolele nu coincid una cu cealalta")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
            Companies = new CompanieOperations(_context, _fileManager).GetAllVM().ToList();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                MailAddress address = new MailAddress(Input.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    IsDriver = Input.IsDriver,
                    EmailConfirmed = true,
                    IsOwner = Input.IsOwner,
                    CompanieRefId = !Input.IsOwner ? -1 : Input.CompanieRefId,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (!Input.IsDriver)
                        await _userManager.AddToRoleAsync(user, Enums.Roles.Admin.ToString());
                    else
                        await _userManager.AddToRoleAsync(user, Enums.Roles.Moderator.ToString());

                    return RedirectToPage("./Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
