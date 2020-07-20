using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Messages.Catalog;
using MyOnlineShop.Ordering.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Messages
{
    public class ProductArchivedConsumer : IConsumer<ProductArchivedMessage>
    {
        private readonly ShoppingCartDbContext shoppingCartDbContext;

        public ProductArchivedConsumer(ShoppingCartDbContext shoppingCartDbContext)
        {
            this.shoppingCartDbContext = shoppingCartDbContext;
        }

        public async Task Consume(ConsumeContext<ProductArchivedMessage> context)
        {
            var shoppingCartItems = await this.shoppingCartDbContext
                .ShoppingCartItems
                .Where(x => x.ProductId == context.Message.Id)
                .ToListAsync();

            if (shoppingCartItems.Any())
            {
                foreach (var cartItem in shoppingCartItems)
                {
                    cartItem.IsArchived = context.Message.IsArchived;
                }

                this.shoppingCartDbContext
                    .ShoppingCartItems
                    .UpdateRange(shoppingCartItems);

                await this.shoppingCartDbContext
                    .SaveChangesAsync();
            }
        }
    }
}
