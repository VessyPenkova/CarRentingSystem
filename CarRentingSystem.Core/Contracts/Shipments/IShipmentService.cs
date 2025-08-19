using CarRentingSystem.Core.Models.Shipment;

namespace CarRentingSystem.Core.Contracts.Shipments
{
    public interface IShipmentService
    {
        // Home page / dashboard
        Task<IEnumerable<ShipmentIndexServiceModel>> LastThreeShipments();

        // Listing & filtering
        Task<ShipmentQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            ShipmentSorting sorting = ShipmentSorting.Newest,
            int currentPage = 1,
            int shipmentsPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNames();
        Task<IEnumerable<ShipmentCategoryServiceModel>> AllCategories();
        Task<bool> CategoryExists(int categoryId);

        // CRUD
        Task<int> Create(
            string title,
            string loadingAddress,
            string deliveryAddress,
            string description,
            string imageUrlShipmentGoogleMaps,
            decimal price,
            int categoryId,
            int? driverId,
            string creatorUserId);

        Task Edit(
            int shipmentId,
            string title,
            string loadingAddress,
            string deliveryAddress,
            string description,
            string imageUrlShipmentGoogleMaps,
            decimal price,
            int categoryId);

        Task Delete(int shipmentId);

        // Details & info
        Task<ShipmentDetailsServiceModel> ShipmentDetailsByShipmentId(int shipmentId);
        Task<bool> Exists(int shipmentId);
        Task<int> GetShipmentCategoryId(int shipmentId);

        // Ownership / business rules
        Task<bool> IsOwner(int shipmentId, string currentUserId);
        Task<bool> IsRented(int shipmentId);
        Task<bool> IsRentedByUserWithId(int shipmentId, string currentUserId);

        Task Rent(int shipmentId, string currentUserId);
        Task Leave(int shipmentId);

        // User scoped lists
        Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByDriverId(int driverId);
        Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByUserId(string userId);
    }
}
