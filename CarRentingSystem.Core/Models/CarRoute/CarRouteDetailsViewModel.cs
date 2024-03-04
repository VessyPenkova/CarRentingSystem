using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.CarRoute
{
    public class CarRouteDetailsViewModel
    {
        public string Title { get; set; } = null!;

        public string PickUpAddress { get; set; } = null!;

        public string DeliveryAddress { get; set; } = null!;

        public string ImageImageUrlRouteGoogleMapsUrl { get; set; } = null!;
    }
}
