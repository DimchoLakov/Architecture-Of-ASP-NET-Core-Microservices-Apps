namespace MyOnlineShop.Statistics.Messages
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using MyOnlineShop.Common.Messages.Catalog;
    using MyOnlineShop.Common.Messages.Ordering;
    using MyOnlineShop.Statistics.Data;
    using MyOnlineShop.Statistics.Data.Models;
    using System.Threading.Tasks;

    public class OrderPlacedConsumer : IConsumer<OrderPlacedMessage>
    {
        private readonly StatisticsDbContext dbContext;

        public OrderPlacedConsumer(StatisticsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<OrderPlacedMessage> context)
        {
            var statistics = await this.dbContext
                .Statistics
                .FirstOrDefaultAsync();

            if (statistics != null)
            {
                statistics.TotalSales =  context.Message.Total;

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
                    TotalProducts = 1,
                    TotalSales = context.Message.Total
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
