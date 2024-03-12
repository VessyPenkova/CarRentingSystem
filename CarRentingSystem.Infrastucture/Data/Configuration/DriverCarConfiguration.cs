using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    internal class DriverCarConfiguration : IEntityTypeConfiguration<DriverCar>
    {
        public void Configure(EntityTypeBuilder<DriverCar> builder)
        {
            builder.HasData(new DriverCar()
            {
                DriverCarId = 1,
                PhoneNumber = "00359123456",
                UserId = "dea1286-c198-4129-b3f3-b89d839581"
            });
        }
    }
}
