using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Exceptions;
using CarRentingSystem.Core.Models.Shipment;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRentingSystem.Core.Services
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
        public async Task<ShipmentsQueryModel> All(string? category = null, string? searchTerm = null,
        ShipmentSorting sorting = ShipmentSorting.Newest, int currentPage = 1, int ShipmentsPerPage = 1)
        {
            var result = new ShipmentsQueryModel();
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
                    ShipmentId = cr.ShipmentId,
                    Title = cr.Title,
                    LoadingAddress = cr.LoadingAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    ImageUrlShipmentGoogleMaps = cr.ImageUrlShipmentGoogleMaps,
                    Price = cr.Price,
                    IsRented = cr.RenterId !=null,
                })
                .ToListAsync();

            result.TotalShipmentCount = await shipments.CountAsync();

            return result;
        }
        public async Task<IEnumerable<ShipmentCategoryModel>> AllCategories()
        {
            return await repo.AllReadonly<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new ShipmentCategoryModel()
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


        public async Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByDriverId(int driverId)
        {
            return await repo.AllReadonly<Shipment>()
                .Where(c => c.DriverId == driverId)
                .Select(c => new ShipmentServiceModel()
                {
                    ShipmentId = c.ShipmentId,
                    LoadingAddress = c.LoadingAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    Title = c.Title,
                    ImageUrlShipmentGoogleMaps = c.ImageUrlShipmentGoogleMaps,
                    Price = c.Price,
                    IsRented = c.RenterId !=null,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ShipmentServiceModel>> AllShipmentsByUserId(string userId)
        {
            return await repo.AllReadonly<Shipment>()
                .Where(c => c.RenterId == userId)
                .Select(c => new ShipmentServiceModel()
                {
                    ShipmentId = c.ShipmentId,
                    Title = c.Title,
                    LoadingAddress = c.LoadingAddress,
                    DeliveryAddress = c.DeliveryAddress,
                    ImageUrlShipmentGoogleMaps = c.ImageUrlShipmentGoogleMaps,
                    Price = c.Price,
                    IsRented = c.RenterId !=null,

                })
                .ToListAsync();
        }


        public async Task<int> Create(ShipmentModel model, int driverId)
        {
            var shipment = new Shipment()
            {
                LoadingAddress = model.LoadingAddress,
                DeliveryAddress = model.DeliveryAddress,
                CategId = model.CategId,
                Description = model.Description,
                ImageUrlShipmentGoogleMaps = model.ImageUrlShipmentGoogleMaps,
                Price = model.Price,
                Title = model.Title,
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

        public async Task Delete(int shipmentId)
        {
            var shipment = await repo.GetByIdAsync<Shipment>(shipmentId);

            await repo.SaveChangesAsync();
        }

        public async Task Edit(int shipmentId, ShipmentModel model)
        {
            var shipment = await repo.GetByIdAsync<Shipment>(shipmentId);

            shipment.Description = model.Description;
            shipment.ImageUrlShipmentGoogleMaps = model.ImageUrlShipmentGoogleMaps;
            shipment.Price = model.Price;
            shipment.Title = model.Title;
            shipment.LoadingAddress = model.LoadingAddress;
            shipment.DeliveryAddress = model.DeliveryAddress;
            shipment.CategId = model.CategId;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> Exists(int shipmentId)
        {
            return await repo.AllReadonly<Shipment>()
                .AnyAsync(cr => cr.ShipmentId == shipmentId );
        }

        public async Task<int> GetShipmentCategoryId(int shipmentId)
        {
            return (await repo.GetByIdAsync<Shipment>(shipmentId)).CategId;
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

        public async Task<ShipmentDetailsModel> ShipmentDetailsByShipmentId(int shipmentId)
        {
            return await repo.AllReadonly<Shipment>()
                .Where(cr => cr.ShipmentId == shipmentId)
                .Select(cr => new ShipmentDetailsModel()
                {
                    LoadingAddress = cr.LoadingAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    Category = cr.Category.Name,
                    Description = cr.Description,
                    ShipmentId = shipmentId,
                    ImageUrlShipmentGoogleMaps = cr.ImageUrlShipmentGoogleMaps,
                    IsRented = cr.RenterId != null,
                    Price = cr.Price,
                    Title = cr.Title,
                    Driver = new Models.Driver.DriverServiceModel()
                    {
                        Email = cr.Driver.User.Email,
                        PhoneNumber = cr.Driver.PhoneNumber
                    }

                })
                .FirstAsync();
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

        public async Task<IEnumerable<ShipmentHomeModel>> LastThreeShipments()
        {
            return await repo.AllReadonly<Shipment>()
                .OrderByDescending(cr => cr.ShipmentId)
                .Select(cr => new ShipmentHomeModel()
                {
                    ShipmentId = cr.ShipmentId, 
                    Title = cr.Title,
                    LoadingAddress = cr.LoadingAddress,
                    DeliveryAddress = cr.DeliveryAddress,
                    ImageUrlShipmentGoogleMapsUrl = cr.ImageUrlShipmentGoogleMaps
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
