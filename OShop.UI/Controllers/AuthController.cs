using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OShop.UI.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        partial class AuthVM
        {
            public string Email { get; set; }
            public string FullName { get; set; }
            public string Password { get; set; }
            public string UserIdentification { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string BuildingInfo { get; set; }
            public string PhoneNumber { get; set; }
            public double CoordX { get; set; }
            public double CoordY { get; set; }
            public bool CompleteProfile { get; set; }
        }
        partial class AuthVMManage
        {
            public string Id { get; set; }
            public bool IsDriver { get; set; } = false;
            public bool IsOwner { get; set; } = false;
            public int RestaurantRefId { get; set; }
        }
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                    UserIdentification = user.UserIdentification,
                };
                if (!string.IsNullOrEmpty(user.UserIdentification))
                {
                    Pass = Password.Generate(10, 0);
                }
                else
                {
                    Pass = user.Password;
                }
                var result = await _userManager.CreateAsync(newAcc, Pass);
                if (result.Succeeded)
                    return Ok("Account created, you can now login.");
                else
                    return Ok("Something went wrong during account creation");

            }
            else
            {
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
                        return Ok(new AuthVM
                        {
                            FullName = account.FullName,
                            BuildingInfo = account.BuildingInfo,
                            PhoneNumber = account.PhoneNumber,
                            Street = account.Street,
                            City = account.City,
                            CompleteProfile = account.CompleteProfile,
                            CoordX = account.CoordX,
                            CoordY = account.CoordY,
                        });
                    }
                    else
                        return Ok("Password is wrong.");
                }
                else if (account.UserIdentification == user.UserIdentification)
                    return Ok(new AuthVM
                    {
                        FullName = account.FullName,
                        BuildingInfo = account.BuildingInfo,
                        PhoneNumber = account.PhoneNumber,
                        Street = account.Street,
                        City = account.City,
                        CompleteProfile = account.CompleteProfile,
                        CoordX = account.CoordX,
                        CoordY = account.CoordY,

                    });
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
                    return Ok(new AuthVMManage
                    {
                        Id = account.IsDriver ? account.Id : null,
                        IsDriver = account.IsDriver,
                        IsOwner = account.IsOwner,
                        RestaurantRefId = account.RestaurantRefId,
                    });
                }
                else
                    return Ok("Password is wrong.");
            }
        }
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
                        account.BuildingInfo = user.BuildingInfo;
                        account.PhoneNumber = user.PhoneNumber;
                        account.Street = user.Street;
                        account.City = user.City;
                        account.CompleteProfile = user.CompleteProfile;
                        account.CoordX = user.CoordX;
                        account.CoordY = user.CoordY;
                        await _userManager.UpdateAsync(account);
                        return Ok("Profile updated.");
                    }
                }
                else if (account.UserIdentification == user.UserIdentification)
                {
                    account.FullName = user.FullName;
                    account.BuildingInfo = user.BuildingInfo;
                    account.PhoneNumber = user.PhoneNumber;
                    account.Street = user.Street;
                    account.City = user.City;
                    account.CompleteProfile = user.CompleteProfile;
                    account.CoordX = user.CoordX;
                    account.CoordY = user.CoordY;

                    await _userManager.UpdateAsync(account);
                    return Ok("Profile updated.");
                }
                return Ok("Data invalid.");
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
