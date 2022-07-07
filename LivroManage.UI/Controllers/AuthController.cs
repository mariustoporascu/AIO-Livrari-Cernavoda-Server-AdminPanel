using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using LivroManage.Database;
using LivroManage.Domain.Models;
using LivroManage.UI.ApiAuth;
using LivroManage.UI.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LivroManage.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly OnlineShopDbContext _context;

        partial class AuthVM : ApplicationUser
        {
            public string Password { get; set; }
            public string NewPassword { get; set; }
            public string FirebaseToken { get; set; }
            public int LocationDeleteId { get; set; }
            public UserLocations Location { get; set; }
            public IEnumerable<UserLocations> Locations { get; set; }
        }
        partial class AuthVMManage : ApplicationUser
        {
            public string FirebaseToken { get; set; }
            public string TelNo { get; set; }
        }
        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            OnlineShopDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
                    if (!string.IsNullOrWhiteSpace(user.FirebaseToken))
                    {
                        var saveToken = new FireBaseTokens
                        {
                            Created = DateTime.UtcNow,
                            FBToken = user.FirebaseToken,
                            UserId = newAcc.Id,
                        };
                        _context.FBTokens.Add(saveToken);
                        await _context.SaveChangesAsync();
                    }
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
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAccount([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            ApplicationUser account;
            if (user.Email == "apple@apple.com")
                account = _userManager.Users.FirstOrDefault(usr => usr.UserIdentification == user.UserIdentification);
            else
                account = await _userManager.FindByEmailAsync(user.Email);
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
                if (!string.IsNullOrEmpty(user.Password))
                {
                    MailAddress address = new MailAddress(user.Email);
                    string userName = address.User;
                    var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(user.FirebaseToken))
                        {
                            var tokenExists = _context.FBTokens.AsNoTracking().FirstOrDefault(tkn => tkn.FBToken == user.FirebaseToken);
                            if (tokenExists == null || tokenExists.UserId != account.Id)
                            {
                                var saveToken = new FireBaseTokens
                                {
                                    Created = DateTime.UtcNow,
                                    FBToken = user.FirebaseToken,
                                    UserId = account.Id,
                                };
                                _context.FBTokens.Add(saveToken);
                                await _context.SaveChangesAsync();
                            }

                        }
                        if (account.LoginTokenExpiry.CompareTo(DateTime.UtcNow) <= 0)
                        {
                            account.LoginToken = Password.Generate(20, 0);
                            account.LoginTokenExpiry = DateTime.UtcNow.AddDays(1);
                            await _userManager.UpdateAsync(account);
                        }

                        return Ok(new AuthVM
                        {
                            FullName = account.FullName,
                            PhoneNumber = account.PhoneNumber,
                            HasSetPassword = account.HasSetPassword,
                            CompleteProfile = account.CompleteProfile,
                            Locations = _context.UserLocations.AsNoTracking().AsEnumerable().Where(loc => loc.UserId == account.Id),
                            LoginToken = account.LoginToken,
                        });
                    }
                    else
                        return Ok("Password is wrong.");
                }
                else if (account.UserIdentification == user.UserIdentification)
                {
                    if (account.LoginTokenExpiry.CompareTo(DateTime.UtcNow) <= 0)
                    {
                        account.LoginToken = Password.Generate(20, 0);
                        account.LoginTokenExpiry = DateTime.UtcNow.AddDays(1);
                        await _userManager.UpdateAsync(account);
                    }
                    if (!string.IsNullOrWhiteSpace(user.FirebaseToken))
                    {
                        var tokenExists = _context.FBTokens.AsNoTracking().FirstOrDefault(tkn => tkn.FBToken == user.FirebaseToken);
                        if (tokenExists == null || tokenExists.UserId != account.Id)
                        {
                            var saveToken = new FireBaseTokens
                            {
                                Created = DateTime.UtcNow,
                                FBToken = user.FirebaseToken,
                                UserId = account.Id,
                            };
                            _context.FBTokens.Add(saveToken);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return Ok(new AuthVM
                    {
                        Email = account.Email,
                        FullName = account.FullName,
                        PhoneNumber = account.PhoneNumber,
                        CompleteProfile = account.CompleteProfile,
                        HasSetPassword = account.HasSetPassword,
                        Locations = _context.UserLocations.AsNoTracking().AsEnumerable().Where(loc => loc.UserId == account.Id),
                        LoginToken = account.LoginToken

                    });
                }

                return Ok("Login data invalid.");
            }
        }
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
                    if (!string.IsNullOrWhiteSpace(user.FirebaseToken))
                    {
                        var tokenExists = _context.FBTokens.AsNoTracking().FirstOrDefault(tkn => tkn.FBToken == user.FirebaseToken);
                        if (tokenExists == null || tokenExists.UserId != account.Id)
                        {
                            var saveToken = new FireBaseTokens
                            {
                                Created = DateTime.UtcNow,
                                FBToken = user.FirebaseToken,
                                UserId = account.Id,
                            };
                            _context.FBTokens.Add(saveToken);
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (account.LoginTokenExpiry.CompareTo(DateTime.UtcNow) <= 0)
                    {
                        account.LoginToken = Password.Generate(20, 0);
                        account.LoginTokenExpiry = DateTime.UtcNow.AddDays(1);
                        await _userManager.UpdateAsync(account);
                    }
                    return Ok(new AuthVMManage
                    {
                        Id = account.IsDriver ? account.Id : null,
                        TelNo = account.PhoneNumber,
                        IsDriver = account.IsDriver,
                        IsOwner = account.IsOwner,
                        CompanieRefId = account.CompanieRefId,
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
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
            else if (account.ResetTokenRetries >= 5)
            {
                return Ok("Tried too many times");

            }
            else
            {
                if (user.ResetTokenPass == account.ResetTokenPass)
                {
                    var result = await _userManager.ResetPasswordAsync(account, account.ResetTokenPassIdentity, user.NewPassword);
                    if (result.Succeeded)
                    {
                        account.ResetTokenPass = "";
                        account.ResetTokenRetries = 0;
                        account.HasSetPassword = true;
                        account.ResetTokenPassIdentity = "";
                        await _userManager.UpdateAsync(account);
                        return Ok("Password changed");
                    }
                }
                account.ResetTokenRetries += 1;
                await _userManager.UpdateAsync(account);
                return Ok("Data invalid.");
            }
        }
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
            else if (!string.IsNullOrWhiteSpace(account.ResetTokenPass) && account.ResetTokenExpiry.CompareTo(DateTime.UtcNow) > 0)
            {
                return Ok("Already generated");
            }
            else
            {
                account.ResetTokenPass = CodeGen.Generate();
                account.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
                account.ResetTokenPassIdentity = await _userManager.GeneratePasswordResetTokenAsync(account);
                account.ResetTokenRetries = 0;
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
                if (!string.IsNullOrEmpty(user.Password))
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
                if (!string.IsNullOrEmpty(user.Password))
                {
                    MailAddress address = new MailAddress(user.Email);
                    string userName = address.User;
                    var result = await _signInManager.PasswordSignInAsync(userName, user.Password, false, false);
                    if (result.Succeeded)
                    {

                        if (user.Location.LocationId > 0)
                        {
                            var locationDb = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == account.Id && loc.LocationId == user.Location.LocationId);
                            if (locationDb != null)
                            {
                                user.Location.UserId = account.Id;
                                user.Location.LocationId = locationDb.LocationId;
                                _context.UserLocations.Update(user.Location);
                            }
                        }

                        else if (_context.UserLocations.AsNoTracking().AsEnumerable().Where(loc => loc.UserId == account.Id).Count() < 3)
                        {
                            user.Location.UserId = account.Id;
                            _context.UserLocations.Add(user.Location);

                        }
                        await _context.SaveChangesAsync();

                        return Ok($"{user.Location.LocationId}:Location updated.");
                    }
                }
                else if (account.UserIdentification == user.UserIdentification)
                {
                    if (user.Location.LocationId > 0)
                    {
                        var locationDb = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.UserId == account.Id && loc.LocationId == user.Location.LocationId);
                        if (locationDb != null)
                        {
                            user.Location.UserId = account.Id;
                            user.Location.LocationId = locationDb.LocationId;
                            _context.UserLocations.Update(user.Location);
                        }
                    }

                    else if (_context.UserLocations.AsNoTracking().AsEnumerable().Where(loc => loc.UserId == account.Id).Count() < 3)
                    {
                        user.Location.UserId = account.Id;
                        _context.UserLocations.Add(user.Location);

                    }
                    await _context.SaveChangesAsync();

                    return Ok($"{user.Location.LocationId}:Location updated.");
                }
                return Ok("Data invalid.");
            }
        }
        [Authorize]
        [HttpPost("deletelocation")]
        public async Task<IActionResult> DeleteLocation([FromBody] object userInfo)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var user = JsonConvert.DeserializeObject<AuthVM>(userInfo.ToString(), settings);
            var account = await _userManager.FindByEmailAsync(user.Email);
            if (account != null)
            {
                var location = _context.UserLocations.AsNoTracking().FirstOrDefault(loc => loc.LocationId == user.LocationDeleteId && loc.UserId == account.Id);
                if (location != null)
                {
                    _context.UserLocations.Remove(location);
                    await _context.SaveChangesAsync();
                    return Ok("Location deleted.");
                }
            }
            return Ok("Data invalid.");
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
                if (!string.IsNullOrWhiteSpace(user.Password))
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
                if (r.Length < 6)
                    r = r + "7";
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
