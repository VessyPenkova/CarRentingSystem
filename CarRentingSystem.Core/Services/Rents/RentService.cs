using CarRentingSystem.Core.Contracts.Rents;
using CarRentingSystem.Core.Models.Shipment;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services.Rent
{
    public class RentService : IRentService
    {
        private readonly IRepository repo;

        public RentService(IRepository _repo)
        {
            repo = _repo;
        }
        public async Task<IEnumerable<RentServiceModel>> All()
        {
            return await repo.AllReadonly<Shipment>()
                .Include(sh => sh.Driver.User)
                .Include(sh => sh.RenterId)
                .Where(sh => sh.RenterId != null)
                .Select(sh => new RentServiceModel()
                {
                    ShipmentTitle = sh.Title,
                    ShipmentImageURL = sh.ImageUrlShipmentGoogleMaps,
                    DriverFullName = sh.Driver.User.UserName,
                    DriverEmail = sh.Driver.User.Email,
                    RenterFullName = sh.RenterId,
                    RenterEmail = sh.RenterId,
                })
               .ToListAsync();
        }
    }
}

