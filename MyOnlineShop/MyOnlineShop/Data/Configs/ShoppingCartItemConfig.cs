using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Data.Models.ShoppingCarts;

namespace MyOnlineShop.Data.Configs
{
    public class ShoppingCartItemConfig : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
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
