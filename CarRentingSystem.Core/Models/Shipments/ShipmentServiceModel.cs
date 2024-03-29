using CarRentingSystem.Core.Contracts.Shipments;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentServiceModel: IShipmentModel
    {
        public int Id { get; set; }
     
        public string Title { get; set; } = null!;
    
        public string LoadingAddress { get; set; } = null!;
     
        public string DeliveryAddress { get; set; } = null!;

        [DisplayName("Image URL")]
        public string ImageUrlShipmentGoogleMaps { get; set; } = null!;

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Is Rented")]
        public bool IsRented { get; init; } 

    }
}
