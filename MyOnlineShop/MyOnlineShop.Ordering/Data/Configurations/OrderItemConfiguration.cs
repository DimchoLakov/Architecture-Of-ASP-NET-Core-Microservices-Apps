using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Ordering.Data.Models.Orders;

namespace MyOnlineShop.Ordering.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Price)
                .HasColumnType("decimal(16,2)");

            builder
                .Property(x => x.ProductPrice)
                .HasColumnType("decimal(16,2)");

            builder
                .Property(x => x.ProductId)
                .IsRequired();

            builder
                .Property(x => x.ProductName)
                .IsRequired();

            builder
                .Property(x => x.ProductPrice)
                .HasColumnType("decimal(16,2)");
        }
    }
}
