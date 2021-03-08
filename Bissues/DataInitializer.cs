
using System;
using Bissues.Models;
using Microsoft.AspNetCore.Identity;

/* Just inject UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager
 * into Startup and call DataInitializer.SeedData passing user and role managers 
 * after authentication */

namespace Bissues
{
    public static class DataInitializer
    {
        public static void SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<AppUser> userManager)
        {
            if(userManager.FindByEmailAsync("admin@admin.com").Result == null)
            {
                AppUser user = new AppUser();
                user.Email = "admin@admin.com";
                user.UserName = "admin@admin.com";
                user.FirstName = "Admin";
                user.LastName = "Istrator";
                user.DisplayName = "Admin";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;
                if(result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
            if(userManager.FindByEmailAsync("user@user.com").Result == null)
            {
                AppUser user = new AppUser();
                user.Email = "user@user.com";
                user.UserName = "user@user.com";
                user.FirstName = "user";
                user.LastName = "user";
                user.DisplayName = "Test User";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;
                if(result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
            if(userManager.FindByEmailAsync("dev@dev.com").Result == null)
            {
                AppUser user = new AppUser();
                user.Email = "dev@dev.com";
                user.UserName = "dev@dev.com";
                user.FirstName = "dev";
                user.LastName = "dev";
                user.DisplayName = "Test Developer";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;
                if(result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Developer").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Developer").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Developer";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}