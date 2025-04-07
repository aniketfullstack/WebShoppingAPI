﻿using Microsoft.AspNetCore.Identity;

namespace WebShoppingAPI.Infrastructure.Models.IdentityModels
{
    public class AppUserRole : IdentityUserRole<string>
    {
        // navigation properties in EF need to be initialised to null so
        // use the null forgiving operator for these
        public virtual AppUser User { get; set; } = null!;
        public virtual AppRole Role { get; set; } = null!;
    }
}
