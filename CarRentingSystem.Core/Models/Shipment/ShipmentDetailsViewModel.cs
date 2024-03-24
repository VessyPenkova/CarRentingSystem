namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentDetailsViewModel
    {
        public string Title { get; set; } = null!;

        public string LoadingAddress { get; set; } = null!;

        public string DeliveryAddress { get; set; } = null!;

        public string ImageUrlShipmentGoogleMapsUrl { get; set; } = null!;
    }
}
