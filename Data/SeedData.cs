using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Identity;

namespace LandingPageBuilder.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager)
        {
            // Create default user if not exists
            var defaultUser = await userManager.FindByEmailAsync("demo@landingpagebuilder.com");
            if (defaultUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "demo",
                    Email = "demo@landingpagebuilder.com",
                    EmailConfirmed = true,
                    FirstName = "Demo",
                    LastName = "User"
                };

                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
