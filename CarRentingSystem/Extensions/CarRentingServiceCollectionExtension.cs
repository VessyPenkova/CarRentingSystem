using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Contracts.Admin;
using CarRentingSystem.Core.Exceptions;
using CarRentingSystem.Core.Services;
using CarRentingSystem.Core.Services.Admin;
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
