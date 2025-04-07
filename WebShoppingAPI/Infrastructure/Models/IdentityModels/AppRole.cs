using Microsoft.AspNetCore.Identity;

namespace WebShoppingAPI.Infrastructure.Models.IdentityModels
{
    public class AppRole : IdentityRole
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
