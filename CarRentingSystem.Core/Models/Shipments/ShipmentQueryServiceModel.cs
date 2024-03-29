namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentQueryServiceModel
    {
        public int TotalShipmentCount { get; set; }

        public IEnumerable<ShipmentServiceModel> Shipments { get; set; } = new List<ShipmentServiceModel>();
    }
}

