using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Infrastructure.Data.Config
{
    public class VariantConfiguration:IEntityTypeConfiguration<Variant>
    {
        public void Configure(EntityTypeBuilder<Variant> builder)
        {
            builder.Property(p => p.Id).IsRequired();

            builder.HasOne(p => p.Product).WithMany()
               .HasForeignKey(p => p.ProductId);

            //builder.HasOne(p => p.ProductSize).WithMany()
            //.HasForeignKey(p => p.ProductSizeId);

            builder.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");



            builder.Property(p => p.VariantSKU);

            //builder.HasOne(p => p.Status).WithMany()
            //    .HasForeignKey(p => p.ProductStatusId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
