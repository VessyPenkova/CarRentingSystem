using CarRentingSystem.Core.Contracts.Drivers;
using CarRentingSystem.Core.Contracts.Rents;
using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Core.Contracts.Users;
using CarRentingSystem.Core.Exceptions;
using CarRentingSystem.Core.Services.Drivers;
using CarRentingSystem.Core.Services.Rent;
using CarRentingSystem.Core.Services.Shipments;
using CarRentingSystem.Core.Services.Users;
using CarRentingSystem.Infrastucture.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CarRentingServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IGuard, Guard>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRentService, RentService>();

            return services;
        }
    }
}
