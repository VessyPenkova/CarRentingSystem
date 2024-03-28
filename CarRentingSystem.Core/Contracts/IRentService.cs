using CarRentingSystem.Core.Models.Shipment;

namespace CarRentingSystem.Core.Contracts
{
    public interface IRentService
    {
        Task<IEnumerable<RentServiceModel>> All();
    }
}
