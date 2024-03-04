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
            services.AddScoped<ICarRouteService, CarRouteService>();
            services.AddScoped<ICarDriverService, DriverCarService>();
            services.AddScoped<IGuard, Guard>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
