using CarRentingSystem.Core.Contracts;

namespace CarRentingSystem.Core.Models.CarRoute
{
    public class CarRouteHomeModel : ICarRouteModel
    {
        public int CarRouteId { get; set; }

        public string Title { get; set; } = null!;

        public string PickUpAddress { get; init; } = null!;

        public string DeliveryAddress { get; init; } = null!;

        public string ImageUrlRouteGoogleMaps { get; set; } = null!;
    }
}
