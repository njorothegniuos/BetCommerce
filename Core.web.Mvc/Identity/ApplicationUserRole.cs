using Microsoft.AspNetCore.Identity;

namespace Core.web.Mvc.Identity
{
    public class ApplicationUserRole : IdentityRole<string>
    {
        public ApplicationUserRole() : base()
        {
        }
      public virtual ApplicationRole Role { get; set; }
    }
}
