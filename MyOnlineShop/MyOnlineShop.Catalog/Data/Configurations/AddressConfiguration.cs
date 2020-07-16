using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Catalog.Data.Models.Customers;

namespace MyOnlineShop.Catalog.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.AddressLine)
                .IsRequired();

            builder
                .Property(x => x.Country)
                .IsRequired();

            builder
                .Property(x => x.PostCode)
                .IsRequired();

            builder
                .Property(x => x.Town)
                .IsRequired();
        }
    }
}
