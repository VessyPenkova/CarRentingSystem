using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {          
            builder.HasData(CreateUser());
        }
        private List<IdentityUser> CreateUser()
        {
            var users = new List<IdentityUser>();
            var hasher = new PasswordHasher<IdentityUser>();

            var user = new IdentityUser()
            {
                Id = "dea1286-c198-4129-b3f3-b89d839581",
                UserName = "agent@mail.com",
                NormalizedUserName = "agent@mail.com",
                Email = "agent@mail.com",
                NormalizedEmail = "agent@mail.com",

            };
            user.PasswordHash =
            hasher.HashPassword(user, "agent123");

            users.Add(user);

            user = new IdentityUser()
            {
                Id = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
            };
            user.PasswordHash =
            hasher.HashPassword(user, "guest123");

            users.Add(user);
            return users;
        }
    }
}
