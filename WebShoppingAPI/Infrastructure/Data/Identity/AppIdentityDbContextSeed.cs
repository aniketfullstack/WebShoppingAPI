using Microsoft.AspNetCore.Identity;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Infrastructure.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var roles = new List<AppRole>
            {
                new AppRole{Name = "SuperUser"},
                new AppRole{Name = "AdminUser"},
                new AppRole{Name = "RegularUser"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }


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
