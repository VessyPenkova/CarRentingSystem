using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Core.Exceptions;
using CarRentingSystem.Core.Models.Drivers;
using CarRentingSystem.Core.Models.Shipment;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRentingSystem.Core.Services.Shipments
{
    public class ShipmentService : IShipmentService
    {
        private readonly IRepository repo;

        private readonly IGuard guard;

        private readonly ILogger logger;

        public ShipmentService(
            IRepository _repo,
            IGuard _guard,
            ILogger<ShipmentService> _logger)
        {
            repo = _repo;
            guard = _guard;
            logger = _logger;
        }

        public async Task<bool> Exists(int shipmentId)
        {
            return await repo.AllReadonly<Shipment>()
                .AnyAsync(cr => cr.ShipmentId == shipmentId);
        }

        public async Task<ShipmentDetailsServiceModel> ShipmentDetailsByShipmentId(int shipmentId)
        {
            return await repo.AllReadonly<Shipment>()
                .Where(sh => sh.ShipmentId == shipmentId)
                .Select(sh => new ShipmentDetailsServiceModel()
                {
                    ShipmentId = shipmentId,
                    LoadingAddress = sh.LoadingAddress,
                    DeliveryAddress = sh.DeliveryAddress,
                    Category = sh.Category.Name,
                    Description = sh.Description,
                    ImageUrlShipmentGoogleMaps = sh.ImageUrlShipmentGoogleMaps,
                    IsRented = sh.RenterId != null,
                    Price = sh.Price,
                    Title = sh.Title,
                    Driver = new DriverServiceModel()
                    {
                        Email = sh.Driver.User.Email,
                        PhoneNumber = sh.Driver.PhoneNumber
                    }
                })
                .FirstAsync();
        }
        public async Task<ShipmentQueryServiceModel> All(string? category = null, string? searchTerm = null,
        ShipmentSorting sorting = ShipmentSorting.Newest, int currentPage = 1, int ShipmentsPerPage = 1)
        {
            var result = new ShipmentQueryServiceModel();
            var shipments = repo.AllReadonly<Shipment>();


            if (string.IsNullOrEmpty(category) == false)
            {
                shipments = shipments
                    .Where(cr => cr.Category.Name == category);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                shipments = shipments
                    .Where(r => EF.Functions.Like(r.Title.ToLower(), searchTerm) ||
                        EF.Functions.Like(r.LoadingAddress.ToLower(), searchTerm) ||
                        EF.Functions.Like(r.DeliveryAddress.ToLower(), searchTerm) ||
                        EF.Functions.Like(r.Description.ToLower(), searchTerm));
            }
            shipments = sorting switch
            {
                ShipmentSorting.Price => shipments
                    .OrderBy(cr => cr.Price),
                ShipmentSorting.NotRentedFirst => shipments
                    .OrderBy(cr => cr.RenterId),
                _ => shipments.OrderByDescending(cr => cr.ShipmentId)
            };

            result.Shipments = await shipments
                .Skip((currentPage - 1) * ShipmentsPerPage)
                .Take(ShipmentsPerPage)
                .Select(cr => new ShipmentServiceModel()
                {
                    Id = cr.ShipmentId,
                    Title = cr.Title,
                    LoadingAddress = cr.LoadingAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    ImageUrlShipmentGoogleMaps = cr.ImageUrlShipmentGoogleMaps,
                    Price = cr.Price,
                    IsRented = cr.RenterId != null,
                })
                .ToListAsync();

            result.TotalShipmentCount = await shipments.CountAsync();

            return result;
        }


        public async Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByDriverId(int driverId)
        {
            var shipments = await repo.AllReadonly<Shipment>()
                .Where(c => c.DriverId == driverId)
                .Select(c => new ShipmentServiceModel()
                {
                    Id = c.ShipmentId,
                    LoadingAddress = c.LoadingAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    Title = c.Title,
                    ImageUrlShipmentGoogleMaps = c.ImageUrlShipmentGoogleMaps,
                    Price = c.Price,
                    IsRented = c.RenterId != null,
                })
                .ToListAsync();
            return shipments;
        }
        public async Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByUserId(string userId)
        {
            var shipments = await repo.AllReadonly<Shipment>()
                .Where(c => c.RenterId == userId)
                .Select(c => new ShipmentServiceModel()
                {
                    Id = c.ShipmentId,
                    Title = c.Title,
                    LoadingAddress = c.LoadingAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    ImageUrlShipmentGoogleMaps = c.ImageUrlShipmentGoogleMaps,
                    Price = c.Price,
                    IsRented = c.RenterId != null,

                })
                .ToListAsync();
            return shipments;
        }



        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<Category>()
                .AnyAsync(c => c.CategoryId == categoryId);
        }
        public async Task<IEnumerable<ShipmentCategoryServiceModel>> AllCategories()
        {
            return await repo.AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new ShipmentCategoryServiceModel()
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
        public async Task<int> GetShipmentCategoryId(int shipmentId)
        {
            return (await repo.GetByIdAsync<Shipment>(shipmentId)).CategId;

        }

        public async Task<int> Create(string title, string loadingAddress, string deliveryAddress,
        string description, string imageUrlShipmentGoogleMaps, decimal price, int categoryId, int driverId)
        {
            var shipment = new Shipment()
            {
                LoadingAddress = loadingAddress,
                DeliveryAddress = deliveryAddress,
                CategId = categoryId,
                Description = description,
                ImageUrlShipmentGoogleMaps = imageUrlShipmentGoogleMaps,
                Price = price,
                Title = title,
                DriverId = driverId,
            };

            try
            {
                await repo.AddAsync(shipment);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(Create), ex);
                throw new ApplicationException("Database failed to save info", ex);
            }

            return shipment.ShipmentId;
        }
        public async Task Edit(int shipmentId, string title, string loadingAddress, string deliveryAddress,
        string description, string imageUrlShipmentGoogleMaps, decimal price, int categoryId)
        {
            var shipment = await repo.GetByIdAsync<Shipment>(shipmentId);

            shipment.ShipmentId = shipmentId;
            shipment.Description = description;
            shipment.ImageUrlShipmentGoogleMaps = imageUrlShipmentGoogleMaps;
            shipment.Price = price;
            shipment.Title = title;
            shipment.LoadingAddress = loadingAddress;
            shipment.DeliveryAddress = deliveryAddress;
            shipment.CategId = categoryId;

            await repo.SaveChangesAsync();
        }
        public async Task Delete(int shipmentId)
        {
            var shipment = await repo.AllReadonly<Shipment>()
                .AnyAsync(sh => sh.ShipmentId == shipmentId);
            await repo.DeleteAsync<Shipment>(shipment);
            await repo.SaveChangesAsync();
        }


        public async Task<bool> HasDriverWithId(int shipmentId, string currentUserId)
        {
            bool result = false;
            var shipment = await repo.AllReadonly<Shipment>()
                .Where(cr => cr.ShipmentId == shipmentId)
                .Include(cr => cr.Driver)
                .FirstOrDefaultAsync();

            if (shipment?.Driver != null && shipment.Driver.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }
        public async Task<bool> IsRented(int shipmentId)
        {
            return (await repo.GetByIdAsync<Shipment>(shipmentId)).RenterId != null;
        }
        public async Task<bool> IsRentedByUserWithId(int shipmentId, string currentUserId)
        {
            bool result = false;
            var shipment = await repo.AllReadonly<Shipment>()
                .Where(cr => cr.ShipmentId == shipmentId)
                .FirstOrDefaultAsync();

            if (shipment != null && shipment.RenterId == currentUserId)
            {
                result = true;
            }

            return result;
        }
        public async Task<IEnumerable<ShipmentIndexServiceModel>> LastThreeShipments()
        {
            return await repo.AllReadonly<Shipment>()
                .OrderByDescending(cr => cr.ShipmentId)
                .Select(cr => new ShipmentIndexServiceModel()
                {
                    ShipmentId = cr.ShipmentId,
                    Title = cr.Title,
                    LoadingAddress = cr.LoadingAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    ImageUrlShipmentGoogleMaps = cr.ImageUrlShipmentGoogleMaps
                })
                .Take(3)
                .ToListAsync();
        }
        public async Task Leave(int shipmentId)
        {
            var shipment = await repo.GetByIdAsync<Shipment>(shipmentId);
            guard.AgainstNull(shipment, "Route can not be found");
            shipment.RenterId = null;

            await repo.SaveChangesAsync();
        }
        public async Task Rent(int shipmentId, string currentUserId)
        {
            var shipment = await repo.GetByIdAsync<Shipment>(shipmentId);

            if (shipment != null && shipment.RenterId != null)
            {
                throw new ArgumentException("Route is not available right now, please try tomorrow");
            }

            guard.AgainstNull(shipment, "Route can not be found");
            shipment.RenterId = currentUserId;

            await repo.SaveChangesAsync();
        }

    }
}
