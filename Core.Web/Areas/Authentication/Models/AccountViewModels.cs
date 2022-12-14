using System.ComponentModel.DataAnnotations;

namespace Core.Web.Areas.Authentication.Models
{
    public class AccountViewModels
    {
        public class AccountLoginModel
        {
            [Required(ErrorMessage = "The Username is required")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public string ReturnUrl { get; set; }
            public bool RememberMe { get; set; }
        }

        public class AccountForgotPasswordModel
        {
            [Required(ErrorMessage = "Email Address is required")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "The Username is required")]
            public string UserName { get; set; }

            public string ReturnUrl { get; set; }
        }

        public class AccountResetPasswordModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string PasswordConfirm { get; set; }
        }

        public class AccountRegistrationModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string UserName { get; set; }

            [Display(Name = "Phone Number")]
            [RegularExpression(@"^(?:[0-9]??){6,14}[0-9]$", ErrorMessage = "Invalid phone number")]
            [MinLength(12, ErrorMessage = "Invalid phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string PasswordConfirm { get; set; }

            public string ReturnUrl { get; set; }
        }

        public class AccountLockScreenModel
        {

            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public string ReturnUrl { get; set; }
            public bool RememberMe { get; set; }
        }

        public class AccountGenericModel
        {
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Email { get; set; }
            public DateTime LastPasswordChangedDate { get; set; }
            public string RecordStatusDescription { get; set; }

            [Display(Name = "Old Password")]
            [Required(ErrorMessage = "Old Password is required")]
            public string OldPassword { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("NewPassword", ErrorMessage = "Passwords provided don't match")]
            public string ConfirmPassword { get; set; }
        }
    }
}
