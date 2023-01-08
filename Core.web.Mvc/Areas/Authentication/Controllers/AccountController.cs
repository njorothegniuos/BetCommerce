using Core.web.Mvc.Controllers;
using Core.web.Mvc.Identity;
using Core.web.Mvc.Models;
using Core.web.Mvc.Utils;
using Core.web.Mvc.Utils.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Core.web.Mvc.Authentication.Models.AccountViewModels;

namespace Core.Web.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMemoryCache _memoryCache;
        IConfiguration _configuration;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
             IConfiguration configuration, IMemoryCache memoryCache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new AccountRegistrationModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountRegistrationModel applicationUser)
        {
            var currentUser = await _userManager.FindByEmailAsync(applicationUser.Email);

            if (currentUser != null)
            {
                TempData["Error"] = "This email already exists!";

                return View(applicationUser);
            }

            ApplicationUser user = new ApplicationUser();
            user.UserName = applicationUser.Email;
            user.Email = applicationUser.Email;
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;
            user.CreatedDate = DateTime.Now;
            user.RecordStatus = (byte)RecordStatus.Approved;
            user.IsEnabled = true;
            user.IsExternalUser = true;
            user.LastPasswordChangedDate = DateTime.Now;
            user.PhoneNumberConfirmed = false;
            user.FirstName = applicationUser.FirstName;
            user.OtherNames = applicationUser.LastName;
            user.LastLoginDateTime = DateTime.Now;
            user.ApprovedDate = DateTime.Now;
            user.TwoFactorEnabled = true;
            user.LockoutEnabled = false;

            //refactor move to app service
            string newPassword = PasswordStore.GenerateRandomPassword(new Core.web.Mvc.Utils.Infrastructure.PasswordOptions
            {
                RequiredLength = int.Parse(_configuration.GetSection("RequiredPasswordLength").Value),
                RequireNonLetterOrDigit = bool.Parse(_configuration.GetSection("PasswordRequireNonLetterOrDigit").Value),
                RequireDigit = bool.Parse(_configuration.GetSection("PasswordRequireDigit").Value),
                RequireLowercase = bool.Parse(_configuration.GetSection("PasswordRequireLowercase").Value),
                RequireUppercase = bool.Parse(_configuration.GetSection("PasswordRequireUppercase").Value),
                RequireNonAlphanumeric = true,
                RequiredUniqueChars = 1
            });

            IdentityResult result = _userManager.CreateAsync
                (user, newPassword).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "StandardUser").Wait();

                //SEND EMAIL
                string subject = applicationUser.UserName + " Login Details";
                var messageBody = $"Welcome to Bet commerce ™. Below you will find your login details.<br/Username: {applicationUser.UserName} <br/> Password: {newPassword}<br/> You can login by clicking {_configuration.GetSection("Web:WebClientUrl").Value}<br/> Regards, <br/> Bet Team";

                CreateEmailRequest request = new CreateEmailRequest("info@bet.com", user.Email, "support@bet.com", subject, messageBody, true, DLRStatus.Pending.ToString(), ServiceOrigin.WebMVC.ToString(), "0", "0", "0");

                using (var pclient = new HttpClient())
                {
                    var api_url = _configuration["WebApi:WebApiClientUrl"];
                    api_url = api_url + "v1/email/publish";

                    pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                    var data = JsonConvert.SerializeObject(request);
                    var content = new StringContent(data, Encoding.UTF8, "application/json"); ;

                    var response = await pclient.PostAsync(api_url, content);

                    var res = await response.Content.ReadAsStringAsync();
                    res = HttpUtility.HtmlDecode(res);

                    var responseData = JsonConvert.DeserializeObject<string>(res);

                    Serilog.Log.Information("Email request received for processing: " + responseData);
                }

                TempData["Success"] = "User details created successfully!";

                return RedirectToAction("Login");
            }
            else
            {
                TempData["Error"] = "Failed to create user! ";
            }



            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // We do not want to use any existing identity information
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            returnUrl = returnUrl ?? Url.Content("~/");
            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginModel viewModel, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            string key = $"user-{viewModel.Email}";
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);


            var user = await _memoryCache.GetOrCreateAsync(key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromHours(24));
                    return _userManager.FindByEmailAsync(viewModel.Email);
                }
                );

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");

                return RedirectToLocal(returnUrl);
            }

            if (user.RecordStatus != (byte)RecordStatus.Approved || !user.IsEnabled)
            {
                ModelState.AddModelError("", "Sorry, your status does not allow you to login");

                return View(viewModel);
            }

            if (user != null)
            {
                await _signInManager.SignOutAsync();
                //check if user is an api user
                var userRole = await _userManager.GetRolesAsync(user);

                if (userRole.Any(x => x.Equals(EnumHelper.GetDescription(WellKnownUserRoles.APIAccount))))
                {
                    ModelState.AddModelError("", "Sorry, your role does not allow you to login");

                    return View(viewModel);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                var signInStatus = await _signInManager.PasswordSignInAsync(user.UserName, viewModel.Password, false, false);

                if (signInStatus.Succeeded)
                {
                    Serilog.Log.Information("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else if (signInStatus.IsLockedOut)
                {
                    //_logger.LogInformation("User logged in.");
                    return View("Lockout");
                }
                else if (signInStatus.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Contact System administrator!");
                    return View(viewModel);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(viewModel);
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //_logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        #region helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(DashboardController.Dashboard), "Dashboard", new { area = "" });
            }
        }

        #endregion
    }
}
