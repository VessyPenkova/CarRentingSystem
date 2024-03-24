using CarRentingSystem.Core.Contracts;
using System.Text;
using System.Text.RegularExpressions;

namespace CarRentingSystem.Core.Extensions
{
    public static  class ModelExtensions
    {
        public static string GetInformation(this IShipmentModel Shipment)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Shipment.Title.Replace(" ", "-"));
            sb.Append("-");
            sb.Append(GetLoadingAddress(Shipment.LoadingAddress));
            sb.Append(GetDeliveryAddress(Shipment.DeliveryAddress));

            return sb.ToString();
        }

        private static string GetLoadingAddress(string loadingAddress)
        {
            string result = string
                .Join("-", loadingAddress.Split(" ",
                StringSplitOptions.RemoveEmptyEntries)
                .Take(3));

            return Regex.Replace(loadingAddress, @"[^a-zA-Z0-9\-]", string.Empty);
        }

        private static string GetDeliveryAddress(string deliveryAddress)
        {
            string result = string
                .Join("-", deliveryAddress.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Take(3));

            return Regex.Replace(deliveryAddress, @"[^a-zA-Z0-9\-]", string.Empty);

        }
    }
}
