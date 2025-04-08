using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Identity;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Infrastructure.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
       
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
           .HasMany(ur => ur.UserRoles)
           .WithOne(u => u.User)
           .HasForeignKey(ur => ur.UserId)
           .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            builder.ApplyConfiguration(new AppRoleConfiguration());
        }
    }
}
