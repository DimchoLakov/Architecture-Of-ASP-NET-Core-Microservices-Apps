using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Models;

namespace MyOnlineShop.Data.Configs
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Price)
                .HasColumnType("decimal(16,2)");

            builder
                .HasOne(x => x.Product);
        }
    }
}
