using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace WebShoppingAPI.Infrastructure.Identity
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
                new AppRole { Id = "e1b649a3-cd6e-4b56-a7f2-f5174b247752", Name = "SuperUser", NormalizedName = "SUPERUSER" },
                new AppRole { Id = "353e1cdb-3d12-474c-9837-5e8e8ffa5462", Name = "AdminUser", NormalizedName = "ADMINUSER" },
                new AppRole { Id = "e2d2b119-e318-44b9-93c7-ff5e9fb27297", Name = "RegularUser", NormalizedName = "REGULARUSER" }
            );
        }
    }
}
