using CarRentingSystem.Core.Models.Shipment;

namespace CarRentingSystem.Core.Contracts.Rents
{
    public interface IRentService
    {
        Task<IEnumerable<RentServiceModel>> All();
    }
}
