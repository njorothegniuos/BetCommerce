using System;
using System.ComponentModel.DataAnnotations;

namespace Core.web.Mvc.Authentication.Models
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
            
            [Required(ErrorMessage = "First name is required")]
            public string FirstName { get; set; } = string.Empty;
            [Required(ErrorMessage = "Last name is required")]
            public string LastName { get; set; } = string.Empty;
            [Required(ErrorMessage = "UserName is required")]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email Address is required")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [MinLength(6, ErrorMessage = "Password cannot be shorter than 6 characters.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]

            [MinLength(6, ErrorMessage = "Password cannot be shorter than 6 characters.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password Must Match")]
            public string PasswordConfirm { get; set; } = string.Empty;

            public string ReturnUrl { get; set; } = string.Empty;
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

        public class AccountGenericModel
        {
            public string UserName { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; } 
            public string Email { get; set; } = string.Empty;
            public DateTime LastPasswordChangedDate { get; set; }
            public string RecordStatusDescription { get; set; } = string.Empty;

            [Display(Name = "Old Password")]
            [Required(ErrorMessage = "Old Password is required")]
            public string OldPassword { get; set; } = string.Empty;
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; } = string.Empty;
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("NewPassword", ErrorMessage = "Passwords provided don't match")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}
