using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.Admin.Services.Statistics;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var statisticsViewModel = await this.statisticsService.GetStatistics();

                return this.View(statisticsViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return View();
        }

        private void HandleException(Exception ex)
        {
            ViewBag.StatisticsInoperativeMsg = $"Statistics Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
