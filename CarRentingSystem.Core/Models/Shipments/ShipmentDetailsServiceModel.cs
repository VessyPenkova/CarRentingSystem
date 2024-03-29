using CarRentingSystem.Core.Models.Drivers;
using System.ComponentModel;

namespace CarRentingSystem.Core.Models.Shipment
{
    public  class ShipmentDetailsServiceModel: ShipmentIndexServiceModel
    {
        public string Description { get; init; } = null!;

        public string Category { get; init; } = null!;

        public DriverServiceModel Driver { get; init; } = null!;

        [DisplayName("Price")]
        public decimal Price { get; init; }

        [DisplayName("Is Rented")]
        public bool IsRented { get; init; }
    }
}
