using Core.Web.Controllers;
using Domain.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Identity;
using Presentation.Identity.Models;
using System.Text;
using System.Web;
using static Core.Web.Areas.Authentication.Models.AccountViewModels;

namespace Core.Web.Areas.Authentication.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IConfiguration _configuration;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
             IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            // EnsureLoggedOut();
            // We do not want to use any existing identity information

            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountRegistrationModel { ReturnUrl = returnUrl };

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegistrationModel user, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = string.Join(" | ", ModelState.Values.SelectMany(a => a.Errors).Select(a => a.ErrorMessage));

                return View(user);
            }

            var existingUser = await _userManager.FindByNameAsync(user.UserName);

            if (existingUser != null)
            {
                TempData["Error"] = "Another user with the username " + user.UserName + " already exists!";

                return View(user);
            }

            var userDTO = new UserRegistrationModel
            {
                UserName = user.UserName,
                Password = user.Password,
                ConfirmPassword = user.Password,
                Email = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            var strRequest = JsonConvert.SerializeObject(userDTO);
            var strResponse = string.Empty;

            //ToDo:Refactor
            using (var pclient = new HttpClient())
            {
                pclient.DefaultRequestHeaders.Add("Accept", "application/json");
                var content = new StringContent(strRequest, Encoding.UTF8, "application/json"); ;

                var response = await pclient.PostAsync(_configuration.GetSection("identity_api_url").Value, content);

                var res = await response.Content.ReadAsStringAsync();
                res = HttpUtility.HtmlDecode(res);

                strResponse = res;
            }

            if (strResponse != null)
            {
                TempData["Success"] = "User account created successfully!<br/>An email will be sent with login credentials once your account has been approved";

                return RedirectToAction("Login");
            }

            TempData["Error"] = "Failed to create user account!";

            return View(user);
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // We do not want to use any existing identity information
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginModel _user, string returnUrl)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(_user);

            // Require the user to have a confirmed email before they can log on.
            var user = await _userManager.FindByNameAsync(_user.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");

                return RedirectToLocal(returnUrl);
            }

            if (user != null)
            {
                await _signInManager.SignOutAsync();
                //check if user is an api user
                var userRole = await _userManager.GetRolesAsync(user);


                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                var signInStatus = await _signInManager.PasswordSignInAsync(_user.Username, _user.Password, false, false);

                if (signInStatus.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else if (signInStatus.RequiresTwoFactor)
                {

                    return RedirectToAction("LoginTwoStep", new { userId = user.Id, ReturnUrl = returnUrl });
                }
                else if (signInStatus.IsLockedOut)
                {
                    //_logger.LogInformation("User logged in.");
                    return View("Lockout");
                }
                else if (signInStatus.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Contact System administrator!");
                    return View(_user);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(_user);
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");

            return View(_user);
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
