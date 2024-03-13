using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Models.Statistics;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services
{
    public class StatisticsService: IStatisticsService
    {
        private readonly IRepository repo;

        public StatisticsService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            int totalCarRoutes = await repo.AllReadonly<CarRoute>()
                .CountAsync(cr => cr.IsActive);
            int rentedCarRoutes = await repo.AllReadonly<CarRoute>()
                .CountAsync(cr => cr.IsActive && cr.RenterId != null);

            return new StatisticsServiceModel()
            {
                TotalCarRoutes = totalCarRoutes,
                TotalRents = rentedCarRoutes
            };
        }
    }
}
