namespace MyOnlineShop.Statistics.DataSeed
{
    using MyOnlineShop.Common.Services;
    using MyOnlineShop.Statistics.Data;
    using MyOnlineShop.Statistics.Data.Models;
    using System.Linq;

    public class StatisticsDataSeeder : IDataSeeder
    {
        private readonly StatisticsDbContext dbContext;

        public StatisticsDataSeeder(StatisticsDbContext dbContext) => this.dbContext = dbContext;

        public void SeedData()
        {
            if (!this.dbContext.Statistics.Any())
            {
                this.dbContext.Statistics.Add(new Statistics
                {
                    // !!!
                    // TotalProducts' initial value is 5 because we're seeding 5 products on startup(if non in the database)
                    TotalProducts = 5,
                    TotalSales = 0
                });

                this.dbContext.SaveChanges();
            }
        }
    }
}
