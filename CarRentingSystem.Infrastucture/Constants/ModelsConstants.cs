using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Infrastucture.Constants
{
    
    public static class ModelsConstants
    {
        public const int NameLength = 50;

        public const int ShipmentTitleMaxLength = 50;

        public const int ShipmentTitleMinLength = 10;

        public const int ShipmentLoadingAddressMaxLength = 150;

        public const int ShipmentLoadingAddressMinLength = 30;

        public const int ShipmentDeliveryAddressMaxLength = 150;

        public const int ShipmentDeliveryMinLength = 30;

        public const int ShipmentDescriptionMaxLength = 500;

        public const int ShipmentDescriptionMinLength = 50;

        public const string ShipmentBookingPriceMinimum = "0.00";

        public const string ShipmentBookingPriceMaximum = "2000.00";

        public const int DriverPhoneMaxLength = 15;

        public const int DriverPhoneMinLength = 7;
    }
}
