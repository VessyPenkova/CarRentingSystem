using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Extensions
{
    public static  class ModelExtensions
    {
        public static string GetInformation(this ICarRouteModel carRoute)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(carRoute.Title.Replace(" ", "-"));
            sb.Append("-");
            sb.Append(GetPickUpAddressAddress(carRoute.PickUpAddress));
            sb.Append(GetDeliveryAddressAddress(carRoute.DeliveryAddress));

            return sb.ToString();
        }

        private static string GetPickUpAddressAddress(string pickUpAddress)
        {
            string result = string
                .Join("-", pickUpAddress.Split(" ",
                StringSplitOptions.RemoveEmptyEntries)
                .Take(3));

            return Regex.Replace(pickUpAddress, @"[^a-zA-Z0-9\-]", string.Empty);
        }

        private static string GetDeliveryAddressAddress(string deliveryUpAddress)
        {
            string result = string
                .Join("-", deliveryUpAddress.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Take(3));

            return Regex.Replace(deliveryUpAddress, @"[^a-zA-Z0-9\-]", string.Empty);

        }
    }
}
