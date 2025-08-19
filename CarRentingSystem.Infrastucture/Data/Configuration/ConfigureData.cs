using Microsoft.AspNetCore.Identity;
using static CarRentingSystem.Infrastucture.Constants.Constants;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class ConfigureData
    {
        public User DriverUser { get; set; } = null!;
        public User GuestUser { get; set; } = null!;
        public User AdminUser { get; set; } = null!;
        public Driver AdminDriver { get; set; } = null!;
        public Driver UserDriver { get; set; } = null!;
        public Category InterCityCategory { get; set; } = null!;
        public Category OneWayCategory { get; set; } = null!;
        public Category RoundShipmentCategory { get; set; } = null!;
        public Category LuxuryCategory { get; set; } = null!;
        public Category CharterCategory { get; set; } = null!;
        public Shipment OneWayShipment { get; set; } = null!;
        public Shipment RoundShipment { get; set; } = null!;
        public Shipment LuxuryShipment { get; set; } = null!;
        public Shipment CharterShipment { get; set; } = null!;

        public ConfigureData()
        {
            ConfigureUsers();
            ConfigureDrivers();
            ConfigureCategories();
            ConfigureShipments();
        }

        private void ConfigureUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();

            DriverUser = new User
            {
                Id = "dea12856-c198-4129-b3f3-b893d8395082",
                UserName = "driver@mail.com",
                NormalizedUserName = "driver@mail.com",
                Email = "driver@mail.com",
                NormalizedEmail = "driver@mail.com",
                FirstName = "Driviot",
                LastName = "Drivilov"
            };
            DriverUser.PasswordHash = hasher.HashPassword(DriverUser, "driver123");

            GuestUser = new User
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
                FirstName = "Gestiot",
                LastName = "Gestiotev"
            };
            GuestUser.PasswordHash = hasher.HashPassword(DriverUser, "guest123");

            AdminUser = new User
            {
                Id = "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                Email = AdminEmail,
                NormalizedEmail = AdminEmail,
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail,
                FirstName = "Adminiot",
                LastName = "Adminov"
            };
            AdminUser.PasswordHash = hasher.HashPassword(DriverUser, "admin123");
        }

        private void ConfigureDrivers()
        {
            UserDriver = new Driver
            {
                DriverId = 1,
                PhoneNumber = "+359770770770",
                UserId = DriverUser.Id
            };
            AdminDriver = new Driver
            {
                DriverId = 2,
                PhoneNumber = "+359770770772",
                UserId = AdminUser.Id
            };
        }

        private void ConfigureCategories()
        {
            InterCityCategory = new Category { CategoryId = 1, Name = "Inter City" };
            OneWayCategory = new Category { CategoryId = 2, Name = "One Way" };
            RoundShipmentCategory = new Category { CategoryId = 3, Name = "Round-Shipment" };
            LuxuryCategory = new Category { CategoryId = 4, Name = "Luxury" };
            CharterCategory = new Category { CategoryId = 5, Name = "Charter" };
        }

        private void ConfigureShipments()
        {
            // choose a real seeded user id:
            var creatorId = AdminUser.Id; // or GuestUser.Id

            this.OneWayShipment = new Shipment
            {
                ShipmentId = 1,
                Title = "One Way",
                LoadingAddress = "Bul. Kniyaginya Maria Luiza, 31, Plovdiv, 4000, BG",
                DeliveryAddress = "Antique Theater, str. Tsar Ivaylo 4, Plovdiv,4000, BG",
                Description = "Tourist in Plovdiv?, this trip is will satisfy your expectation.",
                ImageUrlShipmentGoogleMaps = "https://ekotaxi.bg/wp-content/uploads/2020/03/single_cab_redone-min-1-2048x1536.png",
                Price = 10.00M,
                CategId = InterCityCategory.CategoryId,
                DriverId = UserDriver.DriverId,
                RenterId = this.GuestUser.Id,
                CreatorId = creatorId
            };

            this.RoundShipment = new Shipment
            {
                ShipmentId = 2,
                Title = "Round",
                LoadingAddress = "Bul. Maria Luiza, 31, BG,Plovdiv,  4000",
                DeliveryAddress = "Bul. Alexander Malinov, 78, BG,Sofia,  2000",
                Description = "Go back Plovdiv - Sofia in one hour. The driver will wait.",
                ImageUrlShipmentGoogleMaps = "https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600",
                Price = 50.00M,
                CategId = RoundShipmentCategory.CategoryId,
                DriverId = UserDriver.DriverId,
                CreatorId = creatorId
            };

            this.LuxuryShipment = new Shipment
            {
                ShipmentId = 3,
                Title = "Private-Luxury",
                LoadingAddress = "Bul. Maria Luiza, 31, BG, 4000",
                DeliveryAddress = " Bul. Alexander Malinov, 78, BG, 2000",
                Description = "Busyness trip. This trip is private with a luxury limousine",
                ImageUrlShipmentGoogleMaps = "https://le-cdn.hibuwebsites.com/8978d127e39b497da77df2a4b91f33eb/dms3rep/multi/opt/RSshutterstock_120889072-1920w.jpg",
                Price = 316.80M,
                CategId = LuxuryCategory.CategoryId,
                DriverId = UserDriver.DriverId,
                CreatorId = creatorId
            };

            this.CharterShipment = new Shipment
            {
                ShipmentId = 4,
                Title = "Charter",
                LoadingAddress = "Plovdiv Airport",
                DeliveryAddress = "Sofia Airport",
                Description = "This privet charter. We are here to meet al your expectations",
                ImageUrlShipmentGoogleMaps = "https://www.luxuryaircraftsolutions.com/wp-content/uploads/2020/05/image-226.png",
                Price = 2000.00M,
                CategId = CharterCategory.CategoryId,
                DriverId = UserDriver.DriverId,
                CreatorId = creatorId
            };
        }

    }
}
