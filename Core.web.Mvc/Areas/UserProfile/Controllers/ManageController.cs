﻿using Core.web.Mvc.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using static Core.web.Mvc.Authentication.Models.AccountViewModels;
using System.Security.Cryptography;

namespace Core.web.Mvc.Areas.UserProfile.Controllers
{
    [Area("UserProfile")]
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _applicationUserManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IConfiguration _configuration;

        public ManageController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
             IConfiguration configuration, IMemoryCache memoryCache)
        {
            _signInManager = signInManager;
            _applicationUserManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {

            var applicationUser = await _applicationUserManager.FindByNameAsync(User.Identity.Name);


            var selectedRole = new ApplicationRole();

            var userInfoDTO = new AccountGenericModel()
            {
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                LastPasswordChangedDate = (DateTime)applicationUser.LastPasswordChangedDate,
                CreatedDate = applicationUser.CreatedDate
            };

            return View(userInfoDTO);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public sealed class SecurePasswordHasher
        {
            private const int SaltSize = 16;

            /// <summary>
            /// Size of hash
            /// </summary>
            private const int HashSize = 20;

            /// <summary>
            /// Creates a hash from a password
            /// </summary>
            /// <param name="password">the password</param>
            /// <param name="iterations">number of iterations</param>
            /// <returns>the hash</returns>
            public static string Hash(string password, int iterations)
            {
                //create salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

                //create hash
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                var hash = pbkdf2.GetBytes(HashSize);

                //combine salt and hash
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                //convert to base64
                var base64Hash = Convert.ToBase64String(hashBytes);

                //format hash with extra information
                return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
            }

            public static string Hash(string password)
            {
                return Hash(password, 100);
            }

            public static bool IsHashSupported(string hashString)
            {
                return hashString.Contains("$MYHASH$V1$");
            }

            public static bool Verify(string password, string hashedPassword)
            {
                //check hash
                if (!IsHashSupported(hashedPassword))
                {
                    throw new NotSupportedException("The hashtype is not supported");
                }

                //extract iteration and Base64 string
                var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
                var iterations = int.Parse(splittedHashString[0]);
                var base64Hash = splittedHashString[1];

                //get hashbytes
                var hashBytes = Convert.FromBase64String(base64Hash);

                //get salt
                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                //create hash with given salt
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                //get result
                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
