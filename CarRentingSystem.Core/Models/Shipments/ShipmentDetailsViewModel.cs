using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.Shipments
{
    public class ShipmentDetailsViewModel
    {
        public string LoadingAddress { get; set; } = null!;

        public string DeliveryAddress { get; set; } = null!;

        public string ImageUrlShipmentGoogleMaps { get; set; } = null!;

    }
}
