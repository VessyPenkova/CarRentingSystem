using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var data = new ConfigureData();

            builder.HasData(new User[]
            {
                data.AdminUser,
                data.DriverUser,
                data.GuestUser
            });
        }
    }
}
