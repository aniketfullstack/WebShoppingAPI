using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Config
{
    public class ParentCategoryConfiguration:IEntityTypeConfiguration<ParentCategory>
    {
        public void Configure(EntityTypeBuilder<ParentCategory> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.ParentCategoryName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.ParentCategoryDescription).HasMaxLength(250);
            builder.Property(c => c.UrlSlug);
        }
    }
}
