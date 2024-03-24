using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        private IdentityUser DriverUser;
        private IdentityUser GuestUser;
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            CreateUser();
            builder.HasData(DriverUser, GuestUser);
        }
        private List<IdentityUser> CreateUser()
        {
            var users = new List<IdentityUser>();
            var hasher = new PasswordHasher<IdentityUser>();

            this.DriverUser = new IdentityUser()
            {
                Id = "dea12856-c198-4129-b3f3-b893d8395082",
                UserName = "Driver@mail.com",
                NormalizedUserName = "Driver@mail.com",
                Email = "Driver@mail.com",
                NormalizedEmail = "Driver@mail.com",

            };
            DriverUser.PasswordHash =
            hasher.HashPassword(DriverUser, "Driver123");

            users.Add(DriverUser);

            this.GuestUser = new IdentityUser()
            {
                Id = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
            };
            GuestUser.PasswordHash =
            hasher.HashPassword(GuestUser, "guest123");

            users.Add(GuestUser);
            return users;
        }
    }
}
