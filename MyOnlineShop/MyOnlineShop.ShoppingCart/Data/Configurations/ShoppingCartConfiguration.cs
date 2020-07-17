using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cart = MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;

namespace MyOnlineShop.ShoppingCart.Data.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<Cart.ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<Cart.ShoppingCart> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.UserId)
                .IsRequired();

            builder
                .HasMany(x => x.CartItems)
                .WithOne(x => x.ShoppingCart)
                .HasForeignKey(x => x.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
