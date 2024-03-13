using CarRentingSystem.Core.Models.CarRoute;

namespace CarRentingSystem.Core.Contracts
{
    public interface ICarRouteService
    {
        Task<IEnumerable<CarRouteHomeModel>> LastThreeRoutes();
        Task<IEnumerable<CarRouteCategoryModel>> AllCategories();
        Task<bool> CategoryExists(int categoryId);
        Task<int> Create(CarRouteModel model, int driverCarId);
        Task<CarRoutesQueryModel> All(
            string? category = null,
            string? searchTerm = null,
            CarRouteSorting sorting = CarRouteSorting.Newest,
            int currentPage = 1,
            int routesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNames();
        Task<IEnumerable<CarRouteServiceModel>> AllRoutesByUserId(string userId);
        Task<IEnumerable<CarRouteServiceModel>> AllCarRoutesByDriverId(int driverCarId);
        Task<CarRouteDetailsModel> CarRouteDetailsByCarRouteId(int carRouteId);
        Task<bool> Exists(int carRouteId);
        Task Edit(int carRouteIdId, CarRouteModel model);
        Task<bool> HasDriverCarWithId(int carRouteId, string currentUserId);
        Task<int> GetRouteCategoryId(int carRouteIdId);
        Task Delete(int carRouteId);
        Task<bool> IsRented(int carRouteId);
        Task<bool> IsRentedByUserWithId(int carRouteId, string currentUserId);
        Task Rent(int carRouteId, string currentUserId);
        Task Leave(int carRouteId);
    }
}
