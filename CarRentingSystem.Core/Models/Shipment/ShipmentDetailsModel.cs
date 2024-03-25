using CarRentingSystem.Core.Models.Driver;

namespace CarRentingSystem.Core.Models.Shipment
{
    public  class ShipmentDetailsModel: ShipmentServiceModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public DriverServiceModel Driver { get; set; } 
    }
}
