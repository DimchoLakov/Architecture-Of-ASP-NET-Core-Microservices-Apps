namespace MyOnlineShop.Statistics.Data
{
    using Microsoft.EntityFrameworkCore;
    using MyOnlineShop.Common.Data;
    using MyOnlineShop.Statistics.Data.Models;
    using System.Reflection;

    public class StatisticsDbContext : MessageDbContext
    {
        public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Statistics> Statistics { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
