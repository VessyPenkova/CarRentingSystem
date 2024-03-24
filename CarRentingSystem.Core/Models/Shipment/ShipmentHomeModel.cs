using CarRentingSystem.Core.Contracts;

namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentHomeModel : IShipmentModel
    {
        public int ShipmentId { get; set; }

        public string Title { get; set; } = null!;

        public string LoadingAddress { get; init; } = null!;

        public string DeliveryAddress { get; init; } = null!;

        public string ImageUrlShipmentGoogleMapsUrl { get; set; } = null!;
    }
}
