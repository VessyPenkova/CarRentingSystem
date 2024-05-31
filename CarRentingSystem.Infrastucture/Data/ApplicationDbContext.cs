using CarRentingSystem.Infrastucture.Data.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Infrastucture.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new DriverConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ShipmentConfiguration());

            base.OnModelCreating(builder);
        }
        public DbSet<Shipment> Shipments { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Driver> Drivers { get; set; } = null!;
    }
}