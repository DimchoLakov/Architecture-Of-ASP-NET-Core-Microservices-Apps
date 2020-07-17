using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cart = MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;


namespace MyOnlineShop.ShoppingCart.Data.Configurations
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<Cart.ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<Cart.ShoppingCartItem> builder)
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
