using CarRentingSystem.Core.Models.Shipment;

namespace CarRentingSystem.Models
{
    public class AllShipmentsQueryModel
    {
        public const int ShipmentsPerPage = 3;

        public string? Category { get; set; }

        public string? SearchTerm { get; set; }

        public ShipmentSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;


        public int TotalShipmentsCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<ShipmentServiceModel> Shipments { get; set; } = Enumerable.Empty<ShipmentServiceModel>();
    }
}
