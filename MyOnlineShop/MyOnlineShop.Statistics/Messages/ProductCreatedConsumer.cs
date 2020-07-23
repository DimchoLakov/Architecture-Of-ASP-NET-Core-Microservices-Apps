namespace MyOnlineShop.Statistics.Messages
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using MyOnlineShop.Common.Messages.Catalog;
    using MyOnlineShop.Statistics.Data;
    using MyOnlineShop.Statistics.Data.Models;
    using System.Threading.Tasks;

    public class ProductCreatedConsumer : IConsumer<ProductAddedMessage>
    {
        private readonly StatisticsDbContext dbContext;

        public ProductCreatedConsumer(StatisticsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ProductAddedMessage> context)
        {
            var statistics = await this.dbContext
                .Statistics
                .FirstOrDefaultAsync();

            if (statistics != null)
            {
                statistics.TotalProducts = context.Message.Total;

                this.dbContext
                    .Statistics
                    .Update(statistics);

                await this.dbContext
                    .SaveChangesAsync();
            }
            else
            {
                var newStatistics = new Statistics
                {
                    TotalProducts =  context.Message.Total,
                    TotalSales = 0
                };

                await this.dbContext
                    .Statistics
                    .AddAsync(newStatistics);

                await this.dbContext
                    .SaveChangesAsync();
            }
        }
    }
}
