using CarRentingSystem.Core.Models.Statistics;

namespace CarRentingSystem.Core.Contracts.Statistics
{
    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> Total();
    }
}
