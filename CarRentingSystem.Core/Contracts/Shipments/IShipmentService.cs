﻿using CarRentingSystem.Core.Models.Shipment;
using CarRentingSystem.Infrastucture.Data;

namespace CarRentingSystem.Core.Contracts.Shipments
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentIndexServiceModel>> LastThreeShipments();
        Task<IEnumerable<ShipmentCategoryServiceModel>> AllCategories();
        Task<bool> CategoryExists(int categoryId);
        Task<int> Create(string title, string loadingAddress, string deliveryAddress,
               string description, string imageUrlShipmentGoogleMaps, decimal price,
            int categoryId,int driverId);
        Task<ShipmentQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            ShipmentSorting sorting = ShipmentSorting.Newest,
            int currentPage = 1,
            int shipmentsPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNames();
        Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByUserId(string userId);
        Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByDriverId(int driverId);
        Task<ShipmentDetailsServiceModel> ShipmentDetailsByShipmentId(int shipmentId);
        Task<bool> Exists(int shipmentId);
        Task Edit(int shipmentId, string title, string loadingAddress, string deliveryAddress,
           string description, string imageUrlShipmentGoogleMaps, decimal price, int categoryId);
        Task<bool> HasDriverWithId(int shipmentId, string currentUserId);
        Task<int> GetShipmentCategoryId(int shipmentId);
        Task Delete(int shipmentId);
        Task<bool> IsRented(int shipmentId);
        Task<bool> IsRentedByUserWithId(int shipmentId, string currentUserId);
        Task Rent(int shipmentId, string currentUserId);
        Task Leave(int shipmentId);
    }
}
