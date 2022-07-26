using MassTransit;
using MyOnlineShop.Catalog.Data.Models.Customers;
using MyOnlineShop.Common.Messages.Identity;
using MyOnlineShop.Ordering.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Catalog.Messages
{
    public class UserCreatedMessageConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly CatalogDbContext catalogDbContext;

        public UserCreatedMessageConsumer(CatalogDbContext catalogDbContext)
        {
            this.catalogDbContext = catalogDbContext;
        }

        public async Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            var customer = this.catalogDbContext
                    .Customers
                    .FirstOrDefault(x => x.UserId == context.Message.UserId);

            if (customer == null)
            {
                var newCustomer = new Customer
                {
                    UserId = context.Message.UserId,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    Email = context.Message.Email
                };

                await catalogDbContext
                    .Customers
                    .AddAsync(newCustomer);

                await catalogDbContext
                    .SaveChangesAsync();
            }
        }
    }
}
