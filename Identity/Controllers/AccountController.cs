using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Identity;
using Presentation.Identity.Models;
using static Identity.Identity.Models.AccountViewModels;

namespace Presentation.Controllers
{
    [Route("Account")]
    public  class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("/register")]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            var user = userModel.Adapt<ApplicationUser>();
            user.LockoutEnabled = false;
            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,
                                    "StandardUser");
                //fire email event

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(ModelState.TryAddModelError(error.Code, error.Description));
                }
            }

            return Ok($"User {userModel.Email} created successfully!");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(AccountLoginModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return BadRequest(viewModel);

            // Require the user to have a confirmed email before they can log on.
            var user = await _userManager.FindByEmailAsync(viewModel.Email);


            if (user == null)
            {
                return BadRequest("Invalid login attempt.");
            }

            if (user.LockoutEnabled)
            {
                return BadRequest("Sorry, your status does not allow you to login");
            }

            if (user != null)
            {
                await _signInManager.SignOutAsync();
                //check if user is an api user

                var signInStatus = await _signInManager.PasswordSignInAsync(user.UserName, viewModel.Password, false, false);

                if (signInStatus.Succeeded)
                {
                    return Ok("User logged in.");                 
                }
                else if (signInStatus.IsLockedOut)
                {
                    //_logger.LogInformation("User logged in.");
                    return BadRequest("Lockout");
                }
                else if (signInStatus.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Contact System administrator!");
                    return BadRequest(viewModel);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return BadRequest(viewModel);
                }
            }

            return BadRequest("Invalid login attempt.");
        }
    }
}
