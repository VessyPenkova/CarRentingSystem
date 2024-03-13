using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Exceptions;
using CarRentingSystem.Infrastucture.Data.Common;
using CarRentingSystem.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentingSystem.Core.Models.CarRoute;
using System.Diagnostics;

namespace CarRentingSystem.Core.Services
{
    public class CarRouteService : ICarRouteService
    {
        private readonly IRepository repo;

        private readonly IGuard guard;

        private readonly ILogger logger;

        public CarRouteService(
            IRepository _repo,
            IGuard _guard,
            ILogger<CarRouteService> _logger)
        {
            repo = _repo;
            guard = _guard;
            logger = _logger;
        }
        public async Task<CarRoutesQueryModel> All(string? category = null, string? searchTerm = null,
        CarRouteSorting sorting = CarRouteSorting.Newest, int currentPage = 1, int carRoutesPerPage = 1)
        {
            var result = new CarRoutesQueryModel();
            var carRoutes = repo.AllReadonly<CarRoute>()
                .Where(cr => cr.IsActive);

            if (string.IsNullOrEmpty(category) == false)
            {
                carRoutes = carRoutes
                    .Where(cr => cr.Category.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                carRoutes = carRoutes
                    .Where(r => EF.Functions.Like(r.Title.ToLower(), searchTerm) ||
                        EF.Functions.Like(r.PickUpAddress.ToLower(), searchTerm) ||
                        EF.Functions.Like(r.DeliveryAddress.ToLower(), searchTerm) ||
                        EF.Functions.Like(r.Description.ToLower(), searchTerm));
            }
            carRoutes = sorting switch
            {
                CarRouteSorting.Price => carRoutes
                    .OrderBy(cr => cr.Price),
                CarRouteSorting.NotRentedFirst => carRoutes
                    .OrderBy(cr => cr.RenterId),
                _ => carRoutes.OrderByDescending(cr => cr.CarRouteId)
            };

            result.CarRoutes = await carRoutes
                .Skip((currentPage - 1) * carRoutesPerPage)
                .Take(carRoutesPerPage)
                .Select(cr => new CarRouteServiceModel()
                {
                    CarRouteId = cr.CarRouteId,
                    Title = cr.Title,
                    PickUpAddress = cr.PickUpAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    ImageUrlRouteGoogleMaps = cr.ImageUrlRouteGoogleMaps,
                    Price = cr.Price,
                    IsRented = cr.RenterId !=null,
                })
                .ToListAsync();

            result.TotalCarRouteCount = await carRoutes.CountAsync();

            return result;
        }
        public async Task<IEnumerable<CarRouteCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new CarRouteCategoryModel()
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }
        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<Category>()
                .AnyAsync(c => c.CategoryId == categoryId);
        }




        public async Task<IEnumerable<CarRouteServiceModel>> AllCarRoutesByDriverId(int driverCarId)
        {
            return await repo.AllReadonly<CarRoute>()
                .Where(c => c.IsActive)
                .Where(c => c.DriverCarId == driverCarId)
                .Select(c => new CarRouteServiceModel()
                {
                    CarRouteId = c.CarRouteId,
                    PickUpAddress = c.PickUpAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    Title = c.Title,
                    ImageUrlRouteGoogleMaps = c.ImageUrlRouteGoogleMaps,
                    Price = c.Price,
                    IsRented = c.RenterId !=null,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CarRouteServiceModel>> AllRoutesByUserId(string userId)
        {
            return await repo.AllReadonly<CarRoute>()
                .Where(c => c.RenterId == userId)
                .Where(c => c.IsActive)
                .Select(c => new CarRouteServiceModel()
                {
                    CarRouteId = c.CarRouteId,
                    Title = c.Title,
                    PickUpAddress = c.PickUpAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    ImageUrlRouteGoogleMaps = c.ImageUrlRouteGoogleMaps,
                    Price = c.Price,
                    IsRented = c.RenterId !=null,

                })
                .ToListAsync();
        }


        public async Task<int> Create(CarRouteModel model, int driverCarId)
        {
            var caRoute = new CarRoute()
            {
                PickUpAddress = model.PickUpAddress,
                DeliveryAddress = model.DeliveryAddress,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrlRouteGoogleMaps = model.ImageUrlRouteGoogleMaps,
                Price = model.Price,
                Title = model.Title,
                DriverCarId = driverCarId,                               
            };

            try
            {
                await repo.AddAsync(caRoute);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(Create), ex);
                throw new ApplicationException("Database failed to save info", ex);
            }

            return caRoute.CarRouteId;
        }

        public async Task Delete(int carRouteId)
        {
            var carRoute = await repo.GetByIdAsync<CarRoute>(carRouteId);
            carRoute.IsActive = false;

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int carRouteId, CarRouteModel model)
        {
            var carRoute = await repo.GetByIdAsync<CarRoute>(carRouteId);

            carRoute.Description = model.Description;
            carRoute.ImageUrlRouteGoogleMaps = model.ImageUrlRouteGoogleMaps;
            carRoute.Price = model.Price;
            carRoute.Title = model.Title;
            carRoute.PickUpAddress = model.PickUpAddress;
            carRoute.DeliveryAddress = model.DeliveryAddress;
            carRoute.CategoryId = model.CategoryId;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int carRouteId)
        {
            return await repo.AllReadonly<CarRoute>()
                .AnyAsync(cr => cr.CarRouteId == carRouteId && cr.IsActive);
        }

        public async Task<int> GetRouteCategoryId(int carRouteId)
        {
            return (await repo.GetByIdAsync<CarRoute>(carRouteId)).CategoryId;
        }

        public async Task<bool> HasDriverCarWithId(int carRouteId, string currentUserId)
        {
            bool result = false;
            var carRoute = await repo.AllReadonly<CarRoute>()
                .Where(cr => cr.IsActive)
                .Where(cr => cr.CarRouteId == carRouteId)
                .Include(cr => cr.DriverCar)
                .FirstOrDefaultAsync();

            if (carRoute?.DriverCar != null && carRoute.DriverCar.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<CarRouteDetailsModel> CarRouteDetailsByCarRouteId(int carRouteId)
        {
            return await repo.AllReadonly<CarRoute>()
                .Where(cr => cr.IsActive)
                .Where(cr => cr.CarRouteId == carRouteId)
                .Select(cr => new CarRouteDetailsModel()
                {
                    PickUpAddress = cr.PickUpAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    Category = cr.Category.Name,
                    Description = cr.Description,
                    CarRouteId = carRouteId,
                    ImageUrlRouteGoogleMaps = cr.ImageUrlRouteGoogleMaps,
                    IsRented = cr.RenterId != null,
                    Price = cr.Price,
                    Title = cr.Title,
                    DriverCar = new Models.DriverCar.DriverCarServiceModel()
                    {
                        Email = cr.DriverCar.User.Email,
                        PhoneNumber = cr.DriverCar.PhoneNumber
                    }

                })
                .FirstAsync();
        }
        public async Task<bool> IsRented(int carRouteId)
        {
            return (await repo.GetByIdAsync<CarRoute>(carRouteId)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserWithId(int carRouteId, string currentUserId)
        {
            bool result = false;
            var carRoute = await repo.AllReadonly<CarRoute>()
                .Where(cr => cr.IsActive)
                .Where(cr => cr.CarRouteId == carRouteId)
                .FirstOrDefaultAsync();

            if (carRoute != null && carRoute.RenterId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<CarRouteHomeModel>> LastThreeRoutes()
        {
            return await repo.AllReadonly<CarRoute>()
                .Where(cr => cr.IsActive)
                .OrderByDescending(cr => cr.CarRouteId)
                .Select(cr => new CarRouteHomeModel()
                {
                    CarRouteId = cr.CarRouteId, 
                    Title = cr.Title,
                    PickUpAddress = cr.PickUpAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    ImageUrlRouteGoogleMaps = cr.ImageUrlRouteGoogleMaps
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task Leave(int carRouteId)
        {
            var carRoute = await repo.GetByIdAsync<CarRoute>(carRouteId);
            guard.AgainstNull(carRoute, "Route can not be found");
            carRoute.RenterId = null;

            await repo.SaveChangesAsync();
        }

        public async Task Rent(int carRouteId, string currentUserId)
        {
            var carRoute = await repo.GetByIdAsync<CarRoute>(carRouteId);

            if (carRoute != null && carRoute.RenterId != null)
            {
                throw new ArgumentException("Route is already rented");
            }

            guard.AgainstNull(carRoute, "Route can not be found");
            carRoute.RenterId = currentUserId;

            await repo.SaveChangesAsync();
        }

    }
}
