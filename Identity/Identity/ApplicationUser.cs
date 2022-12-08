using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = string.Empty;

    }
}
