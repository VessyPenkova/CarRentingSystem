namespace CarRentingSystem.Core.Contracts
{
    public interface ICarRouteModel
    {
        public string Title { get; }

        public string PickUpAddress { get; }

        public string DeliveryAddress { get;  }
    }
}
