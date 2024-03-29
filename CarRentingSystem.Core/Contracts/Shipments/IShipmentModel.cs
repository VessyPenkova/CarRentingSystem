namespace CarRentingSystem.Core.Contracts.Shipments
{
    public interface IShipmentModel
    {
        public string Title { get; }

        public string LoadingAddress { get; }

        public string DeliveryAddress { get; }
    }
}
