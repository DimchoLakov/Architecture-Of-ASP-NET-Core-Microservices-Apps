using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Data.Models.ShoppingCarts;

namespace MyOnlineShop.Data.Configs
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasMany(x => x.CartItems)
                .WithOne(x => x.ShoppingCart)
                .HasForeignKey(x => x.ShoppingCartId);
        }
    }
}
