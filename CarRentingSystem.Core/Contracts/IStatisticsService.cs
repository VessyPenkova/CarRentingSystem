using CarRentingSystem.Core.Models.Statistics;

namespace CarRentingSystem.Core.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> Total();
    }
}
