using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Config
{
    public class CategoryConfiguration:IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.CategoryDescription);

            //builder.HasOne(c => c.ParentCategory).WithMany()
            //   .HasForeignKey(c => c.ParentId);

            //builder.HasOne(c => c.Status).WithMany()
            //    .HasForeignKey(c => c.CategoryStatusId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
