using CarRentingSystem.Infrastucture.Data.Configuration;
using CarRentingSystem.Infrastucture.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {      
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            var data = new ConfigureData();

            builder.HasData(new Driver[] {
            data.AdminDriver, 
            data.UserDriver
            });
        }
    }
}
