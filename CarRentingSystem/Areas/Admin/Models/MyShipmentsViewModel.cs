using CarRentingSystem.Core.Models.Shipment;

namespace CarRentingSystem.Areas.Admin.Models
{

    public class MyShipmentsViewModel
    {
        public IEnumerable<ShipmentServiceModel> AddedShipments { get; set; }
            = new List<ShipmentServiceModel>();

        public IEnumerable<ShipmentServiceModel> RentedShipments { get; set; }
            = new List<ShipmentServiceModel>();
    }

}