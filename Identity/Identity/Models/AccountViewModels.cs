using System.ComponentModel.DataAnnotations;

namespace Identity.Identity.Models
{
    public class AccountViewModels
    {
        public class AccountLoginModel
        {
            [Required(ErrorMessage = "Email Address is required")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public string ReturnUrl { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }

        public class AccountForgotPasswordModel
        {
            [Required(ErrorMessage = "Email Address is required")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "The Username is required")]
            public string UserName { get; set; } = string.Empty;

            public string ReturnUrl { get; set; } = string.Empty;
        }

        public class AccountResetPasswordModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string PasswordConfirm { get; set; } = string.Empty;
        }

        public class AccountRegistrationModel
        {
            public string Username { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Compare("Email")]
            public string EmailConfirm { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string PasswordConfirm { get; set; } = string.Empty;
        }

        public class AccountLockScreenModel
        {

            public string Username { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public string ReturnUrl { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }
    }
}
