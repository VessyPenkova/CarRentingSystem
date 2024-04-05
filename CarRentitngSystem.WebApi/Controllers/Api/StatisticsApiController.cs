using CarRentingSystem.Core.Contracts.Statistics;
using CarRentingSystem.Core.Models.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentitngSystem.WebApi.Controllers.Api
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService service;
        public StatisticsApiController(IStatisticsService _service)
        {
            this.service = _service;
        }
        /// <summary>
        /// Get statistics about count of rented cars and available routes
        /// </summary>
        /// <returns>Total routes and  and booked shipments</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200,Type = typeof(StatisticsServiceModel))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStatistics()
        {
            var model = await service.Total();

            return Ok(model);
        }
    }
}
