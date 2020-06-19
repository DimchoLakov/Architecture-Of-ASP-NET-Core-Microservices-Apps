using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Data.Models.Products;

namespace MyOnlineShop.Data.Configs
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Price)
                .HasColumnType("decimal(16,2)");

            builder
                .Property(x => x.Name)
                .IsRequired();
        }
    }
}
