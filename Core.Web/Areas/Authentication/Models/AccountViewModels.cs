using System.ComponentModel.DataAnnotations;

namespace Core.Web.Areas.Authentication.Models
{
    public class AccountViewModels
    {
        public class AccountLoginModel
        {
            [Required(ErrorMessage = "The Username is required")]
            public string Username { get; set; } = string.Empty;

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
            [Required]
            public string FirstName { get; set; } = string.Empty;
            [Required]
            public string LastName { get; set; } = string.Empty;
            [Required]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
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
