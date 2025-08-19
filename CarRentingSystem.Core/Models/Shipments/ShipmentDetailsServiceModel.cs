using CarRentingSystem.Core.Models.Drivers;
using CarRentingSystem.Infrastucture.Data;
using System.ComponentModel;

namespace CarRentingSystem.Core.Models.Shipment
{
    public  class ShipmentDetailsServiceModel: ShipmentIndexServiceModel
    {
        public int ShipmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LoadingAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrlShipmentGoogleMaps { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsRented { get; set; }

        public string CreatorId { get; set; } = null!;



        public DriverServiceModel? Driver { get; set; }

        // If you use slugged info in routes:
        public string GetInformation() => $"{Title}-{Category}".Replace(' ', '-');
        public string? RenterId { get; set; }

    }
}
