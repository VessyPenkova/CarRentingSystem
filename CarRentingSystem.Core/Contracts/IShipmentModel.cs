namespace CarRentingSystem.Core.Contracts
{
    public interface IShipmentModel
    {
        public string Title { get; }

        public string LoadingAddress { get; }

        public string DeliveryAddress { get;  }
    }
}
