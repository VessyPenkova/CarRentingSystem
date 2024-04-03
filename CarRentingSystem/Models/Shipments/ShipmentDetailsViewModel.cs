namespace CarRentingSystem.Models.Shipments
{
    public class ShipmentDetailsViewModel
    {
        public int Id { get; init; }

        public string Title { get; set; }

        public string LoadingAddress { get; set; } = null!;

        public string DeliveryAddress { get; set; } = null!;

        public string ImageUrlShipmentGoogleMapsUrl { get; set; } = null!;
    }
}
