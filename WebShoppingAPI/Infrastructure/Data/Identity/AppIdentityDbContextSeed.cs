using Microsoft.AspNetCore.Identity;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Infrastructure.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Aniket",
                    Email = "aniketvg16@gmail.com",
                    UserName = "aniketvg16@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Aniket",
                        LastName = "Gaikwad",
                        Street = "8 Milton Street",
                        City = "Sheffield",
                        PostCode = "S14JU"
                    }
                };

                await userManager.CreateAsync(user, "Welcome@1234");
                await userManager.AddToRoleAsync(user, "SuperUser");

            }

        }
    }
}
