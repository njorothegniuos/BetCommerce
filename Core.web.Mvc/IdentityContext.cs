using Core.web.Mvc.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.web.Mvc
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>();
            modelBuilder.Entity<ApplicationRole>();

            modelBuilder.Entity<ApplicationUserRole>();
            modelBuilder.Entity<IdentityUserClaim<string>>();
            modelBuilder.Entity<IdentityUserLogin<string>>();
            modelBuilder.Entity<IdentityRoleClaim<string>>();
            modelBuilder.Entity<IdentityUserToken<string>>();
        }
    }
}
