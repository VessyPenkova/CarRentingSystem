using CarRentingSystem.Core.Contracts.Statistics;
using CarRentingSystem.Core.Models.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService statistics)
        {
            this.statistics = statistics;
        }

        [HttpGet]
        public async Task<StatisticsServiceModel> GetStatistics()
          => await this.statistics.Total();
    }
}

