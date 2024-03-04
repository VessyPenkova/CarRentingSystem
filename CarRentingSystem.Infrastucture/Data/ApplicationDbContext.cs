using CarRentingSystem.Infrastucture.Data.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Infrastucture.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new DriverCarConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CarRouteConfiguration());

            base.OnModelCreating(builder);
        }
        public DbSet<CarRoute> CarRoutes { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<DriverCar> DriversCars { get; set; } = null!;
    }
}



