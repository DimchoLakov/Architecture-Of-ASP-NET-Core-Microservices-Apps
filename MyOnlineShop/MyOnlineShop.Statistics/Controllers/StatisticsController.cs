using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.ViewModels.Statistics;
using MyOnlineShop.Statistics.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Statistics.Controllers
{
    [Authorize(Roles = AuthConstants.AdministratorRoleName)]
    public class StatisticsController : ApiController
    {
        private readonly StatisticsDbContext dbContext;

        public StatisticsController(StatisticsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<StatisticsViewModel>> Get()
        {
            return this.Ok(await this.dbContext
                .Statistics
                .Select(x => new StatisticsViewModel
                {
                    TotalProducts = x.TotalProducts,
                    TotalSales = x.TotalSales
                })
                .FirstOrDefaultAsync());
        }
    }
}
