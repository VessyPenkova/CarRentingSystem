namespace CarRentingSystem.Core.Models.Shipment
{
    public class RentServiceModel
    {
        public string ShipmentTitle { get; init; } = null!;
        public string ShipmentImageURL { get; init; } = null!;
        public string DriverFullName { get; init; } = null!;
        public string DriverEmail { get; init; } = null!;
        public string RenterFullName { get; init; } = null!;
        public string RenterEmail { get; init; } = null!;
    }
}
