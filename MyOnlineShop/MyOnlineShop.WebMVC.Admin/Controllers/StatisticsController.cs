using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.WebMVC.Admin.Services.Statistics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return View();
        }

        private void HandleException(Exception ex)
        {
            ViewBag.StatisticsInoperativeMsg = $"Statistics Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
