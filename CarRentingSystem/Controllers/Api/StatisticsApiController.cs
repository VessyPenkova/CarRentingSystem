using CarRentingSystem.Core.Contracts.Statistics;
using CarRentingSystem.Core.Models.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Controllers.Api
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
          => await statistics.Total();
    }
}

