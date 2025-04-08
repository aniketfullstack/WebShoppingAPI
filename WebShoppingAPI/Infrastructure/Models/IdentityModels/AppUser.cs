using Microsoft.AspNetCore.Identity;
using System.Net;

namespace WebShoppingAPI.Infrastructure.Models.IdentityModels
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
