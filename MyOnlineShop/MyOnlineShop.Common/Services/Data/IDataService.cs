using MyOnlineShop.Common.Data.Models;
using System.Threading.Tasks;

namespace MyOnlineShop.Common.Services.Data
{
    public interface IDataService<in TEntity>
        where TEntity : class
    {
        Task MarkMessageAsPublished(int id);

        Task Save(TEntity entity, params Message[] messages);
    }
}
