using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Models;

namespace MyOnlineShop.Data.Configs
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.DeliveryCost)
                .HasColumnType("decimal(16,2)");

            builder
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasOne(x => x.DeliveryAddress);
        }
    }
}
