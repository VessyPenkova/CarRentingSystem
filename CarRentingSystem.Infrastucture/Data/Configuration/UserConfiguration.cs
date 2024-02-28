using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>//IdentityUser
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
               .Property(p => p.IsActive)
               .HasDefaultValue(true);

            builder.HasData(CreateUser());
        }
        private List<ApplicationUser> CreateUser()
        {
            var users = new List<ApplicationUser>();
            var hasher = new PasswordHasher<ApplicationUser>();

            var carDriver = new ApplicationUser()
            {
                Id = "dea1286-c198-4129-b3f3-b89d839581",
                UserName = "agent@mail.com",
                NormalizedUserName = "agent@mail.com",
                Email = "agent@mail.com",
                NormalizedEmail = "agent@mail.com",

            };
            carDriver.PasswordHash =
            hasher.HashPassword(carDriver, "agent123");

            users.Add(carDriver);

            var guest = new ApplicationUser()
            {
                Id = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
            };
            guest.PasswordHash =
            hasher.HashPassword(guest, "guest123");

            users.Add(guest);
            return users;
        }
    }
}
