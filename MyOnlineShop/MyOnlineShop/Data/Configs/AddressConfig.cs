using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Models.Customers;

namespace MyOnlineShop.Data.Configs
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.AddressLine)
                .IsRequired();

            builder
                .Property(x => x.PostCode)
                .IsRequired();

            builder
                .Property(x => x.Town)
                .IsRequired();

            builder
                .Property(x => x.Country)
                .IsRequired();
        }
    }
}
