using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.Shipment
{
    public  class ShipmentCoreFormModel : IShipmentModel
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

        public int CategoryId { get; init; } 

        public string Description { get; init; } = null!;

        public IEnumerable<ShipmentCategoryServiceModel> ShipmentCategories { get; set; } = new List<ShipmentCategoryServiceModel>();

    }
}
