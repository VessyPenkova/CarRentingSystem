using CarRentingSystem.Infrastucture.Data;
using Microsoft.AspNetCore.Identity;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    internal class ConfigureData
    {
        public IdentityUser DriverUser { get; set; }

        public IdentityUser GuestUser { get; set; }

        public Driver Driver { get; set; }

        public Category InterCityCategory { get; set; }

        public Category OneWayCategory { get; set; }

        public Category RoundShipmentCategory { get; set; }

        public Category LuxuryCategory { get; set; }

        public Category CharterCategory { get; set; }

        public Shipment OneWayShipment { get; set; }

        public Shipment RoundShipment { get; set; }

        public Shipment LuxuryShipment { get; set; }

        public Shipment CharterShipment { get; set; }

        public ConfigureData()
        {
            ConfigureUsers();
            ConfigureDriver();
            ConfigureCategories();
            ConfigureShipments();
        }
        private void ConfigureUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();

            DriverUser = new IdentityUser()
            {
                Id = "dea12856-c198-4129-b3f3-b893d8395082",
                UserName = "driver@mail.com",
                NormalizedUserName = "driver@mail.com",
                Email = "driver@mail.com",
                NormalizedEmail = "driver@mail.com"
            };

            DriverUser.PasswordHash =
                 hasher.HashPassword(DriverUser, "driver123");

            GuestUser = new IdentityUser()
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com"
            };

            GuestUser.PasswordHash =
            hasher.HashPassword(GuestUser, "guest123");
        }
        private void ConfigureDriver()
        {
            Driver = new Driver()
            {
                DriverId = 1,
                PhoneNumber = "+359770770770",
                UserId = DriverUser.Id
            };
        }
        private void ConfigureCategories()
        {
            InterCityCategory = new Category()
            {
                CategoryId = 1,
                Name = "Inter City"
            };

            OneWayCategory = new Category()
            {
                CategoryId = 2,
                Name = "One Way"
            };

            RoundShipmentCategory = new Category()
            {
                CategoryId = 3,
                Name = "Round-Shipment"
            };
            LuxuryCategory = new Category()
            {
                CategoryId = 4,
                Name = "Luxury"
            };

            CharterCategory = new Category()
            {
                CategoryId = 5,
                Name = "Charter"
            };
        }
        private void ConfigureShipments()
        {
            OneWayShipment = new Shipment()
            {
                ShipmentId = 1,
                Title = "One Way",
                LoadingAddress = "Bul. Kniyaginya Maria Luiza, 31, Plovdiv, 4000, BG",
                DeliveryAddress = "Antique Theater, str. Tsar Ivaylo 4, Plovdiv,4000, BG",
                Description = "Tourist in Plovdiv?, this trip is will satisfy your expectation.",
                ImageUrlShipmentGoogleMaps = "https://ekotaxi.bg/wp-content/uploads/2020/03/single_cab_redone-min-1-2048x1536.png",
                Price = 10.00M,
                CategId = InterCityCategory.CategoryId,
                DriverId = Driver.DriverId,
            };
            RoundShipment = new Shipment()
            {
                ShipmentId = 2,
                Title = "Round",
                LoadingAddress = "Bul. Maria Luiza, 31, BG,Plovdiv,  4000",
                DeliveryAddress = "Bul. Alexander Malinov, 78, BG,Sofia,  2000",
                Description = "Go back Plovdiv - Sofia in one hour. The driver will wait.",
                ImageUrlShipmentGoogleMaps = "https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600",
                Price = 50.00M,
                CategId = RoundShipmentCategory.CategoryId,
                DriverId = Driver.DriverId
            };
            LuxuryShipment = new Shipment()
            {
                ShipmentId = 3,
                Title = "Private-Luxury",
                LoadingAddress = "Bul. Maria Luiza, 31, BG, 4000",
                DeliveryAddress = " Bul. Alexander Malinov, 78, BG, 2000",
                Description = "Busyness trip. This trip is private with a luxury limousine",
                ImageUrlShipmentGoogleMaps = "https://le-cdn.hibuwebsites.com/8978d127e39b497da77df2a4b91f33eb/dms3rep/multi/opt/RSshutterstock_120889072-1920w.jpg",
                Price = 316.80M,
                CategId = LuxuryCategory.CategoryId,
                DriverId = Driver.DriverId
            };
            CharterShipment = new Shipment()
            {
                ShipmentId =4,
                Title = "Charter",
                LoadingAddress = "Plovdiv Airport",
                DeliveryAddress = "Sofia Airport",
                Description = "This privet charter. We are here to meet al your expectations",
                ImageUrlShipmentGoogleMaps = "https://www.luxuryaircraftsolutions.com/wp-content/uploads/2020/05/image-226.png",
                Price = 2000.00M,
                CategId = CharterCategory.CategoryId,
                DriverId = Driver.DriverId
            };
        }
    }
}

