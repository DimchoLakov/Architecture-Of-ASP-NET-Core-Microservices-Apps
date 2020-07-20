using MyOnlineShop.Common.ViewModels.Statistics;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Admin.Services.Statistics
{
    public interface IStatisticsService
    {
        [Get("/Statistics")]
        Task<StatisticsViewModel> GetStatistics();
    }
}
