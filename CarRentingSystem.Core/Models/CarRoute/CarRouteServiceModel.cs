using CarRentingSystem.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.CarRoute
{
    public class CarRouteServiceModel: ICarRouteModel
    {
        public int CarRouteId { get; init; }

        public string Title { get; init; } = null!;

        public string PickUpAddress { get; init; } = null!;

        public string DeliveryAddress { get; init; } = null!;

        [Display(Name = "Image URL")]
        public string ImageUrlRouteGoogleMaps { get; init; } = null!;

        [Display(Name = "Price per trip")]
        public decimal Price { get; init; }

        [Display(Name = "Is Rented")]
        public bool IsRented { get; init; }
    }
}
