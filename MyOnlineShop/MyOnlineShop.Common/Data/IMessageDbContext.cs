namespace MyOnlineShop.Common.Data
{
    using Microsoft.EntityFrameworkCore;
    using MyOnlineShop.Common.Data.Models;

    public interface IMessageDbContext
    {
        DbSet<Message> Messages { get; set; }
    }
}
