using CarRentingSystem.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentServiceModel: IShipmentModel
    {
        public int ShipmentId { get; init; }

        public string Title { get; init; } = null!;

        public string LoadingAddress { get; init; } = null!;

        public string DeliveryAddress { get; init; } = null!;

        [Display(Name = "Image URL")]
        public string ImageUrlShipmentGoogleMaps { get; init; } = null!;

        [Display(Name = "Price")]
        public decimal Price { get; init; }

        [Display(Name = "Is Rented")]
        public bool IsRented { get; init; }
    }
}
