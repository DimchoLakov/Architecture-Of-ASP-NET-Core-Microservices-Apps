using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOnlineShop.Common.Data.Models;
using System;

namespace MyOnlineShop.Common.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property<string>("serializedData")
                .IsRequired()
                .HasField("serializedData");

            builder
                .Property(x => x.Type)
                .IsRequired()
                .HasConversion(
                    x => x.AssemblyQualifiedName,
                    x => Type.GetType(x));
        }
    }
}
