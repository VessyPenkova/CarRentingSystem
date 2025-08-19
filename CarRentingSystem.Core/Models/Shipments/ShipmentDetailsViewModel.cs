using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.Shipments
{
    public class ShipmentDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Loading address")]
        public string LoadingAddress { get; set; } = string.Empty;

        [Display(Name = "Delivery address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        // keep the name your controller uses:
        public string ImageUrlShipmentGoogleMapsUrl { get; set; } = string.Empty;
    }
}
