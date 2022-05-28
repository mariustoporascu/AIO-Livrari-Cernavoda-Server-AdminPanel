using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OShop.Database;
using OShop.Domain.Models;
using OShop.UI.ApiAuth;
using OShop.UI.Extras;
using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OShop.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        partial class AuthVM
        {
            public string Email { get; set; }
            public string FullName { get; set; }
            public string Password { get; set; }
            public string NewPassword { get; set; }
            public string UserIdentification { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string BuildingInfo { get; set; }
            public string PhoneNumber { get; set; }
            public double CoordX { get; set; }
            public double CoordY { get; set; }
            public string ResetTokenPass { get; set; }
            public bool CompleteProfile { get; set; }
            public bool CompleteLocation { get; set; }
            public bool HasSetPassword { get; set; }
            public string LoginToken { get; set; }
        }
        partial class AuthVMManage
        {
            public string Id { get; set; }
            public bool IsDriver { get; set; } = false;
            public bool IsOwner { get; set; } = false;
            public int RestaurantRefId { get; set; }
            public string LoginToken { get; set; }
        }
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                string Pass;
                MailAddress address = new MailAddress(user.Email);
                string userName = address.User;
                var newAcc = new ApplicationUser
                {
                    Email = user.Email,
                    UserName = userName,
                    FullName = user.FullName,
                    HasSetPassword = string.IsNullOrWhiteSpace(user.Password) ? false : true,
                    UserIdentification = user.UserIdentification,
                };
                IdentityResult result;
                if (!string.IsNullOrEmpty(user.UserIdentification))
                {
                    newAcc.EmailConfirmed = true;
                    result = await _userManager.CreateAsync(newAcc);
                }
                else
                {
                    newAcc.ResetTokenPass = CodeGen.Generate();
                    result = await _userManager.CreateAsync(newAcc, user.Password);
                }
                if (result.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(user.UserIdentification))
                    {
                        EmailSender sender = new EmailSender(_config);
                        sender.SendEmail(newAcc.Email, "Confirmare email - Livro", $"Codul tau pentru confirmarea emailului este : {newAcc.ResetTokenPass}");
                    }
                    await _userManager.AddToRoleAsync(newAcc, Enums.Roles.Customer.ToString());
                    return Ok("Account created, you can now login.");
                }
                else
                    return Ok("Something went wrong during account creation");

            }
            else
            {
                if (!string.IsNullOrEmpty(account.UserIdentification) && account.UserIdentification == user.UserIdentification)
                    return Ok("Reused previous loginwithothers.");
                return Ok("User already exists.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAccount([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else if (account.AccessFailedCount > 0)
            {
                return BadRequest();
            }
            else if (!account.EmailConfirmed)
            {
                return Ok("Email not confirmed");
            }
            else
            {
                if (account.IsDriver || account.IsOwner)
                {
                    return Ok("Login data invalid.");
                }
                if (string.IsNullOrEmpty(account.UserIdentification))
                {
                    MailAddress address = new MailAddress(user.Email);
                    string userName = address.User;
                    var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        account.LoginToken = Password.Generate(20, 0);
                        await _userManager.UpdateAsync(account);
                        return Ok(new AuthVM
                        {
                            FullName = account.FullName,
                            BuildingInfo = account.BuildingInfo,
                            PhoneNumber = account.PhoneNumber,
                            HasSetPassword = account.HasSetPassword,
                            CompleteLocation = account.CompleteLocation,
                            Street = account.Street,
                            City = account.City,
                            CompleteProfile = account.CompleteProfile,
                            CoordX = account.CoordX,
                            CoordY = account.CoordY,
                            LoginToken = account.LoginToken
                        });
                    }
                    else
                        return Ok("Password is wrong.");
                }
                else if (account.UserIdentification == user.UserIdentification)
                {
                    account.LoginToken = Password.Generate(20, 0);
                    await _userManager.UpdateAsync(account);
                    return Ok(new AuthVM
                    {
                        FullName = account.FullName,
                        BuildingInfo = account.BuildingInfo,
                        PhoneNumber = account.PhoneNumber,
                        Street = account.Street,
                        City = account.City,
                        CompleteProfile = account.CompleteProfile,
                        HasSetPassword = account.HasSetPassword,
                        CompleteLocation = account.CompleteLocation,
                        CoordX = account.CoordX,
                        CoordY = account.CoordY,
                        LoginToken = account.LoginToken

                    });
                }

                return Ok("Login data invalid.");
            }
        }

        [HttpPost("loginManage")]
        public async Task<IActionResult> LoginAccountManage([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (!account.IsOwner && !account.IsDriver)
                {
                    return Ok("Login data invalid.");
                }
                MailAddress address = new MailAddress(user.Email);
                string userName = address.User;
                var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                if (result.Succeeded)
                {
                    account.LoginToken = Password.Generate(20, 0);
                    await _userManager.UpdateAsync(account);
                    return Ok(new AuthVMManage
                    {
                        Id = account.IsDriver ? account.Id : null,
                        IsDriver = account.IsDriver,
                        IsOwner = account.IsOwner,
                        RestaurantRefId = account.RestaurantRefId,
                        LoginToken = account.LoginToken
                    });
                }
                else
                    return Ok("Password is wrong.");
            }
        }
        [Authorize]
        [HttpPost("setpassword")]
        public async Task<IActionResult> SetPassword([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (account.UserIdentification == user.UserIdentification)
                {

                    var result = await _userManager.AddPasswordAsync(account, user.Password);
                    if (result.Succeeded)
                        account.HasSetPassword = true;
                    await _userManager.UpdateAsync(account);
                    return Ok("Password set.");
                }
                return Ok("Data invalid.");
            }
        }
        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                MailAddress address = new MailAddress(user.Email);
                string userName = address.User;
                var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                if (result.Succeeded)
                {
                    var passChanged = await _userManager.ChangePasswordAsync(account, user.Password, user.NewPassword);
                    if (passChanged.Succeeded)
                        return Ok("Password changed.");
                }
                return Ok("Data invalid.");
            }
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (user.ResetTokenPass == account.ResetTokenPass)
                {
                    var result = await _userManager.ResetPasswordAsync(account, account.ResetTokenPassIdentity, user.NewPassword);
                    if (result.Succeeded)
                    {
                        account.ResetTokenPass = "";
                        account.ResetTokenPassIdentity = "";
                        await _userManager.UpdateAsync(account);
                        return Ok("Password changed");
                    }
                }

                return Ok("Data invalid.");
            }
        }
        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (user.ResetTokenPass == account.ResetTokenPass)
                {
                    account.ResetTokenPass = "";
                    account.EmailConfirmed = true;
                    await _userManager.UpdateAsync(account);
                    return Ok("Email Confirmed.");
                }

                return Ok("Data invalid.");
            }
        }
        [HttpPost("sendtokenpassword")]
        public async Task<IActionResult> SendTokenPassword([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                account.ResetTokenPass = CodeGen.Generate();
                account.ResetTokenPassIdentity = await _userManager.GeneratePasswordResetTokenAsync(account);
                await _userManager.UpdateAsync(account);
                EmailSender sender = new EmailSender(_config);
                sender.SendEmail(account.Email, "Resetare Parola - Livro", $"Codul tau pentru resetarea parolei este : {account.ResetTokenPass}");
                return Ok("Token sent.");
            }
        }
        [Authorize]
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (string.IsNullOrEmpty(account.UserIdentification))
                {
                    MailAddress address = new MailAddress(user.Email);
                    string userName = address.User;
                    var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        account.FullName = user.FullName;
                        account.PhoneNumber = user.PhoneNumber;
                        account.CompleteProfile = user.CompleteProfile;
                        await _userManager.UpdateAsync(account);
                        return Ok("Profile updated.");
                    }
                }
                else if (account.UserIdentification == user.UserIdentification)
                {
                    account.FullName = user.FullName;
                    account.PhoneNumber = user.PhoneNumber;
                    account.CompleteProfile = user.CompleteProfile;

                    await _userManager.UpdateAsync(account);
                    return Ok("Profile updated.");
                }
                return Ok("Data invalid.");
            }
        }
        [Authorize]
        [HttpPost("userlocation")]
        public async Task<IActionResult> UpdateLocation([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (string.IsNullOrEmpty(account.UserIdentification))
                {
                    MailAddress address = new MailAddress(user.Email);
                    string userName = address.User;
                    var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        account.BuildingInfo = user.BuildingInfo;
                        account.Street = user.Street;
                        account.City = user.City;
                        account.CompleteLocation = user.CompleteLocation;
                        account.CoordX = user.CoordX;
                        account.CoordY = user.CoordY;
                        await _userManager.UpdateAsync(account);
                        return Ok("Location updated.");
                    }
                }
                else if (account.UserIdentification == user.UserIdentification)
                {
                    account.BuildingInfo = user.BuildingInfo;
                    account.Street = user.Street;
                    account.City = user.City;
                    account.CompleteLocation = user.CompleteLocation;
                    account.CoordX = user.CoordX;
                    account.CoordY = user.CoordY;

                    await _userManager.UpdateAsync(account);
                    return Ok("Location updated.");
                }
                return Ok("Data invalid.");
            }
        }

        [Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAcc([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account == null)
            {
                return Ok("Email is wrong or user not existing.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(account.UserIdentification))
                {
                    MailAddress address = new MailAddress(user.Email);
                    string userName = address.User;
                    var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                    if (result.Succeeded)
                        await _userManager.DeleteAsync(account);
                    return Ok($"result : {result.Succeeded.ToString()}");
                }
                else
                {
                    if (account.UserIdentification == user.UserIdentification)
                        await _userManager.DeleteAsync(account);
                    return Ok($"result : {(account.UserIdentification == user.UserIdentification).ToString()}");
                }

            }
        }
        public static class CodeGen
        {
            public static string Generate()
            {
                Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6");
                return r;
            }
        }
        public static class Password
        {
            private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

            public static string Generate(int length, int numberOfNonAlphanumericCharacters)
            {
                if (length < 1 || length > 128)
                {
                    throw new ArgumentException(nameof(length));
                }

                if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
                {
                    throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
                }

                using (var rng = RandomNumberGenerator.Create())
                {
                    var byteBuffer = new byte[length];

                    rng.GetBytes(byteBuffer);

                    var count = 0;
                    var characterBuffer = new char[length];

                    for (var iter = 0; iter < length; iter++)
                    {
                        var i = byteBuffer[iter] % 87;

                        if (i < 10)
                        {
                            characterBuffer[iter] = (char)('0' + i);
                        }
                        else if (i < 36)
                        {
                            characterBuffer[iter] = (char)('A' + i - 10);
                        }
                        else if (i < 62)
                        {
                            characterBuffer[iter] = (char)('a' + i - 36);
                        }
                        else
                        {
                            characterBuffer[iter] = Punctuations[i - 62];
                            count++;
                        }
                    }

                    if (count >= numberOfNonAlphanumericCharacters)
                    {
                        return new string(characterBuffer);
                    }

                    int j;
                    var rand = new Random();

                    for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                    {
                        int k;
                        do
                        {
                            k = rand.Next(0, length);
                        }
                        while (!char.IsLetterOrDigit(characterBuffer[k]));

                        characterBuffer[k] = Punctuations[rand.Next(0, Punctuations.Length)];
                    }

                    return new string(characterBuffer);
                }
            }
        }
    }
}
