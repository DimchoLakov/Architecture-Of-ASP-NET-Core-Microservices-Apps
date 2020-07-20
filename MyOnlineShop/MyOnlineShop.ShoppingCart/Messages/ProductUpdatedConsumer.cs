using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Messages.Catalog;
using MyOnlineShop.Ordering.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Messages
{
    public class ProductUpdatedConsumer : IConsumer<ProductUpdatedMessage>
    {
        private readonly ShoppingCartDbContext shoppingCartDbContext;
        private readonly IMapper mapper;

        public ProductUpdatedConsumer(
            ShoppingCartDbContext shoppingCartDbContext,
            IMapper mapper)
        {
            this.shoppingCartDbContext = shoppingCartDbContext;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ProductUpdatedMessage> context)
        {
            var shoppingCartItems = await this.shoppingCartDbContext
                .ShoppingCartItems
                .Where(x => x.ProductId == context.Message.ProductId)
                .ToListAsync();

            if (shoppingCartItems.Any())
            {
                foreach (var cartItem in shoppingCartItems)
                {
                    this.mapper.Map(context.Message, cartItem);
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
