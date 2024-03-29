using CarRentingSystem.Core.Contracts.Statistics;
using CarRentingSystem.Core.Models.Statistics;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services.Statistic
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository repo;

        public StatisticsService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            int totalShipments = await repo.AllReadonly<Shipment>()
            .CountAsync(s => s.IsActive);

            int totalExecutedShipments = await repo.AllReadonly<Shipment>()
                .CountAsync(s => s.IsActive && s.RenterId != null);

            return new StatisticsServiceModel()
            {
                TotalShipments = totalShipments,
                TotalExecutedShipments = totalExecutedShipments
            };
        }
    }
}
