using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Config
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.BrandName).IsRequired().HasMaxLength(50);
        }
    }
}
