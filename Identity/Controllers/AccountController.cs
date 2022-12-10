using Application.Email.DTOs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Presentation.Identity;
using Presentation.Identity.Models;
using RabbitMQ.Services.Interface;
using RabbitMQ.Utility;
using Swagger.Models.Attribute;
using static Identity.Identity.Models.AccountViewModels;

namespace Presentation.Controllers
{
    /// <summary>
    /// Represents the Account controller.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/account"), SwaggerOrder("A")]
    public  class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;
        private static IMessageQueueService<EmailRequest>? _messageQueueService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _rabbitMQConfiguration = new RabbitMQConfiguration(_configuration["RabbitMqQueueSettings:Uri"],
                              _configuration["RabbitMqQueueSettings:Port"], _configuration["RabbitMqQueueSettings:HostName"],
                              _configuration["RabbitMqQueueSettings:UserName"], _configuration["RabbitMqQueueSettings:Password"],
                              _configuration["RabbitMqQueueSettings:VirtualHost"]);
            _messageQueueService = new MessageQueueService<EmailRequest>(_configuration["RabbitMqQueueSettings:EmailRequestPath"], _rabbitMQConfiguration);
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="userModel">The UserRegistrationModel.</param>
        /// <returns>Returns ok for a successful user creation.</returns>
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string? confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                EmailRequest emailRequest = new EmailRequest(user.Email,"Account confirmation", confirmationLink);
                _messageQueueService.Send(emailRequest);
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

        /// <summary>
        /// Valids a given token and email.
        /// </summary>
        /// <param name="viewModel">The AccountLoginModel.</param>
        /// <returns>Ok for a successfull login</returns>
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

        /// <summary>
        /// Valids a given token and email.
        /// </summary>
        /// <param name="token">The token value.</param>
        /// <param name="email">The email value.</param>
        /// <returns>Ok for a valid token and email.</returns>
        [HttpGet("/confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
                return BadRequest("Error");
            else
                return Ok("Success");
        }
    }
}
