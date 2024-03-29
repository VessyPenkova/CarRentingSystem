using CarRentingSystem.Core.Contracts.Shipments;

namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentIndexServiceModel : IShipmentModel
    {
        public int ShipmentId { get; set; }

        public string Title { get; set; } = null!;

        public string LoadingAddress { get; init; } = null!;

        public string DeliveryAddress { get; init; } = null!;

        public string ImageUrlShipmentGoogleMaps { get; set; } = null!;
    }
}
