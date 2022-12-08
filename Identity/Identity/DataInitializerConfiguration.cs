using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Presentation.Identity;

namespace Identity.Identity
{
    public class DataInitializerConfiguration
    {
        IdentityContext _identityContext;
        public DataInitializerConfiguration(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }
        public async void Initialize()
        {
            string[] roles = new string[] { "StandardUser", "Administrator", "SuperAdministrator" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(_identityContext);

                if (!_identityContext.Roles.Any(r => r.Name == role))
                {
                   await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            ApplicationUser user = new ApplicationUser();
            user.UserName = "sam";
            user.Email = "sam@bet.com";
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;
            user.PhoneNumber = "254707318620";
            user.PhoneNumberConfirmed = true;
            user.FirstName = "Sam";
            user.LockoutEnabled = false;


            if (!_identityContext.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "Abc.123!");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(_identityContext);
                var result = userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Administrator");
            }

            await _identityContext.SaveChangesAsync();
        }


    }
}
