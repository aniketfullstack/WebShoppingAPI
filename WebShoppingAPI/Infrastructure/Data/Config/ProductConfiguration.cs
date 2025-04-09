using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.ProductName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.ProductDescription).IsRequired();
            //builder.Property(p => p.ShortName).IsRequired();
            //builder.HasOne(p => p.Brand).WithMany()
            //    .HasForeignKey(p => p.BrandId);
            //builder.HasOne(p => p.Category).WithMany()
            //  .HasForeignKey(p => p.CategoryId);
        }
    }
}
