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
        private readonly ILogger<ShipmentService> logger;

        public ShipmentService(IRepository repo, IGuard guard, ILogger<ShipmentService> logger)
        {
            this.repo = repo;
            this.guard = guard;
            this.logger = logger;
        }

        // ========== Queries ==========

        public async Task<bool> Exists(int shipmentId)
            => await repo.AllReadonly<Shipment>()
                         .AnyAsync(s => s.ShipmentId == shipmentId);

        public async Task<ShipmentDetailsServiceModel> ShipmentDetailsByShipmentId(int shipmentId)
            => await repo.AllReadonly<Shipment>()
                         .Where(s => s.ShipmentId == shipmentId)
                         .Select(s => new ShipmentDetailsServiceModel
                         {
                             ShipmentId = s.ShipmentId,
                             Title = s.Title,
                             LoadingAddress = s.LoadingAddress,
                             DeliveryAddress = s.DeliveryAddress,
                             Description = s.Description,
                             Category = s.Category.Name,
                             ImageUrlShipmentGoogleMaps = s.ImageUrlShipmentGoogleMaps,
                             Price = s.Price,
                             IsRented = s.RenterId != null,
                             RenterId = s.RenterId,
                             CreatorId = s.CreatorId,
                             Driver = s.Driver == null
                                ? new DriverServiceModel() // empty when not assigned yet
                                : new DriverServiceModel
                                {
                                    Email = s.Driver.User.Email,
                                    PhoneNumber = s.Driver.PhoneNumber
                                }
                         })
                         .FirstAsync();
        //creatorId

        public async Task<ShipmentQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            ShipmentSorting sorting = ShipmentSorting.Newest,
            int currentPage = 1,
            int shipmentsPerPage = 1)
        {
            var result = new ShipmentQueryServiceModel();

            var shipments = repo.AllReadonly<Shipment>();

            if (!string.IsNullOrWhiteSpace(category))
            {
                shipments = shipments.Where(s => s.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = $"%{searchTerm.Trim().ToLower()}%";
                shipments = shipments.Where(s =>
                    EF.Functions.Like(s.Title.ToLower(), term) ||
                    EF.Functions.Like(s.LoadingAddress.ToLower(), term) ||
                    EF.Functions.Like(s.DeliveryAddress.ToLower(), term) ||
                    EF.Functions.Like(s.Description.ToLower(), term));
            }

            shipments = sorting switch
            {
                ShipmentSorting.Price => shipments.OrderBy(s => s.Price),
                ShipmentSorting.NotRentedFirst => shipments.OrderBy(s => s.RenterId),
                _ => shipments.OrderByDescending(s => s.ShipmentId)
            };

            result.Shipments = await shipments
                .Skip((currentPage - 1) * shipmentsPerPage)
                .Take(shipmentsPerPage)
                .Select(s => new ShipmentServiceModel
                {
                    Id = s.ShipmentId,
                    Title = s.Title,
                    LoadingAddress = s.LoadingAddress,
                    DeliveryAddress = s.DeliveryAddress,
                    ImageUrlShipmentGoogleMaps = s.ImageUrlShipmentGoogleMaps,
                    Price = s.Price,
                    IsRented = s.RenterId != null
                })
                .ToListAsync();

            result.TotalShipmentCount = await shipments.CountAsync();
            return result;
        }

        public async Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByDriverId(int driverId)
            => await repo.AllReadonly<Shipment>()
                         .Where(s => s.DriverId == driverId)
                         .Select(s => new ShipmentServiceModel
                         {
                             Id = s.ShipmentId,
                             Title = s.Title,
                             LoadingAddress = s.LoadingAddress,
                             DeliveryAddress = s.DeliveryAddress,
                             ImageUrlShipmentGoogleMaps = s.ImageUrlShipmentGoogleMaps,
                             Price = s.Price,
                             IsRented = s.RenterId != null
                         })
                         .ToListAsync();

        public async Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByUserId(string userId)
            => await repo.AllReadonly<Shipment>()
                         .Where(s => s.RenterId == userId)
                         .Select(s => new ShipmentServiceModel
                         {
                             Id = s.ShipmentId,
                             Title = s.Title,
                             LoadingAddress = s.LoadingAddress,
                             DeliveryAddress = s.DeliveryAddress,
                             ImageUrlShipmentGoogleMaps = s.ImageUrlShipmentGoogleMaps,
                             Price = s.Price,
                             IsRented = s.RenterId != null
                         })
                         .ToListAsync();

        public async Task<IEnumerable<string>> AllCategoriesNames()
            => await repo.AllReadonly<Category>()
                         .OrderBy(c => c.Name)
                         .Select(c => c.Name)
                         .ToListAsync();

        public async Task<IEnumerable<ShipmentCategoryServiceModel>> AllCategories()
            => await repo.AllReadonly<Category>()
                         .OrderBy(c => c.Name)
                         .Select(c => new ShipmentCategoryServiceModel
                         {
                             CategoryId = c.CategoryId,
                             Name = c.Name
                         })
                         .ToListAsync();

        public async Task<bool> CategoryExists(int categoryId)
            => await repo.AllReadonly<Category>()
                         .AnyAsync(c => c.CategoryId == categoryId);

        public async Task<int> GetShipmentCategoryId(int shipmentId)
            => (await repo.GetByIdAsync<Shipment>(shipmentId))!.CategId;

        public async Task<IEnumerable<ShipmentIndexServiceModel>> LastThreeShipments()
            => await repo.AllReadonly<Shipment>()
                         .OrderByDescending(s => s.ShipmentId)
                         .Select(s => new ShipmentIndexServiceModel
                         {
                             ShipmentId = s.ShipmentId,
                             Title = s.Title,
                             LoadingAddress = s.LoadingAddress,
                             DeliveryAddress = s.DeliveryAddress,
                             ImageUrlShipmentGoogleMaps = s.ImageUrlShipmentGoogleMaps
                         })
                         .Take(3)
                         .ToListAsync();

        // ========== Ownership / renting rules ==========

        public async Task<bool> IsOwner(int shipmentId, string currentUserId)
        {
            var creatorId = await repo.AllReadonly<Shipment>()
                                      .Where(s => s.ShipmentId == shipmentId)
                                      .Select(s => s.CreatorId)
                                      .FirstOrDefaultAsync();

            return creatorId != null && creatorId == currentUserId;
        }

        public async Task<bool> IsRented(int shipmentId)
            => (await repo.GetByIdAsync<Shipment>(shipmentId))!.RenterId != null;

        public async Task<bool> IsRentedByUserWithId(int shipmentId, string currentUserId)
        {
            var renterId = await repo.AllReadonly<Shipment>()
                                     .Where(s => s.ShipmentId == shipmentId)
                                     .Select(s => s.RenterId)
                                     .FirstOrDefaultAsync();
            return renterId != null && renterId == currentUserId;
        }

        public async Task Rent(int shipmentId, string currentUserId)
        {
            var entity = await repo.GetByIdAsync<Shipment>(shipmentId);
            guard.AgainstNull(entity, "Shipment can not be found");

            if (entity!.RenterId != null)
                throw new ArgumentException("Route is not available right now, please try tomorrow");

            entity.RenterId = currentUserId;
            await repo.SaveChangesAsync();
        }

        public async Task Leave(int shipmentId)
        {
            var entity = await repo.GetByIdAsync<Shipment>(shipmentId);
            guard.AgainstNull(entity, "Route can not be found");

            entity!.RenterId = null;
            await repo.SaveChangesAsync();
        }

        // ========== CRUD ==========

        public async Task<int> Create(
            string title,
            string loadingAddress,
            string deliveryAddress,
            string description,
            string imageUrlShipmentGoogleMaps,
            decimal price,
            int categoryId,
            int? driverId,
            string creatorUserId)
        {
            var entity = new Shipment
            {
                Title = title,
                LoadingAddress = loadingAddress,
                DeliveryAddress = deliveryAddress,
                Description = description,
                ImageUrlShipmentGoogleMaps = imageUrlShipmentGoogleMaps,
                Price = price,
                CategId = categoryId,
                DriverId = driverId,     // may be null until someone becomes driver
                CreatorId = creatorUserId,
                IsActive = true
            };

            try
            {
                await repo.AddAsync(entity);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Create shipment failed");
                throw new ApplicationException("Database failed to save info", ex);
            }

            return entity.ShipmentId;
        }

        public async Task Edit(
            int shipmentId,
            string title,
            string loadingAddress,
            string deliveryAddress,
            string description,
            string imageUrlShipmentGoogleMaps,
            decimal price,
            int categoryId)
        {
            var entity = await repo.GetByIdAsync<Shipment>(shipmentId);
            guard.AgainstNull(entity, "Shipment not found");

            entity!.Title = title;
            entity.LoadingAddress = loadingAddress;
            entity.DeliveryAddress = deliveryAddress;
            entity.Description = description;
            entity.ImageUrlShipmentGoogleMaps = imageUrlShipmentGoogleMaps;
            entity.Price = price;
            entity.CategId = categoryId;

            await repo.SaveChangesAsync();
        }

        public async Task Delete(int shipmentId)
        {
            var entity = await repo.GetByIdAsync<Shipment>(shipmentId);
            guard.AgainstNull(entity, "Shipment not found");

            await repo.DeleteAsync<Shipment>(shipmentId);  // explicit generic + id
            await repo.SaveChangesAsync();
        }
    }
}
