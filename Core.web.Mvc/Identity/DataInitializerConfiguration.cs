using Core.web.Mvc.Utils;
using Microsoft.AspNetCore.Identity;
using System;

namespace Core.web.Mvc.Identity
{
    public static class DataInitializerConfiguration
    {

        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("sam").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Sam@bet.com".ToUpper();
                user.Email = "Sam@bet.com".ToUpper();
                user.NormalizedEmail = "Sam@bet.com".ToUpper();
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = false;
                user.CreatedDate = DateTime.Now;
                user.RecordStatus = (byte)RecordStatus.Approved;
                user.IsEnabled = true;
                user.IsExternalUser = false;
                user.LastPasswordChangedDate = DateTime.Now;
                user.PhoneNumber = "254707318620";
                user.PhoneNumberConfirmed = true;
                user.FirstName = "Sam";
                user.OtherNames = "Mburu";
                user.LastLoginDateTime = DateTime.Now;
                user.ApprovedDate = DateTime.Now;
                user.IsExternalUser = true;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;

                IdentityResult result = userManager.CreateAsync
                (user, "Abc.123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "StandardUser").Wait();
                }
            }


            if (userManager.FindByNameAsync("su").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Sami@bet.com".ToUpper();
                user.Email = "Sami@bet.com".ToUpper();
                user.NormalizedEmail = "Sami@bet.com".ToUpper();
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = false;
                user.CreatedDate = DateTime.Now;
                user.RecordStatus = (byte)RecordStatus.Approved;
                user.IsEnabled = true;
                user.IsExternalUser = false;
                user.LastPasswordChangedDate = DateTime.Now;
                user.PhoneNumber = "254707318620";
                user.PhoneNumberConfirmed = true;
                user.FirstName = "Sami";
                user.OtherNames = "Mburu";
                user.LastLoginDateTime = DateTime.Now;
                user.ApprovedDate = DateTime.Now;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;
                IdentityResult result = userManager.CreateAsync
                (user, "Abc.123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "SuperAdministrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("StandardUser").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "StandardUser";
                role.Description = "Perform normal operations.";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("SuperAdministrator").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "SuperAdministrator";
                role.Description = "Perform all the operations.";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }

    public static class ApplicationDomainName
    {
        public static string CurrentDomainName { get; set; }
    }
}
