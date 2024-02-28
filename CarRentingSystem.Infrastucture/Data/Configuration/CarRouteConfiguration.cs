using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class CarRouteConfiguration : IEntityTypeConfiguration<CarRoute>
    {
        public void Configure(EntityTypeBuilder<CarRoute> builder)
        {
            builder.HasData(CreateCarRoutes());
        }
        private List<CarRoute> CreateCarRoutes()
        {
            var carRoutes = new List<CarRoute>()
            {
                new CarRoute()
                {
                    CarRouteId = 11,
                    Title ="Private Luxury",
                    PickUpAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                    DeliveryAddress = "Bulgaria, Sofia, Bul, Alexander Malinov, 78",
                    Description = "Whether you want a tourist tour from Plovdiv to Sofia, or simply busyness trip, this trip is private with a luxury limousine",
                    ImageUrlRouteGoogleMaps ="https://le-cdn.hibuwebsites.com/8978d127e39b497da77df2a4b91f33eb/dms3rep/multi/opt/RSshutterstock_120889072-1920w.jpg",
                    Price = 316.80M,
                    CategoryId = 5,
                    DriverCarId = 1,
                    RenterId = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",               
                },
                new CarRoute()
                {
                    CarRouteId = 22,
                    Title ="Shared",
                    PickUpAddress = "Bulgaria, Sofia, Bul, Alexander malinov, 78",
                    DeliveryAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                    Description = "Whether you want a tourist tour from Sofia to Plovdiv, or simply busyness trip, this trip is private with a luxury limousine",
                    ImageUrlRouteGoogleMaps ="https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600",
                    Price = 316.80M,
                    CategoryId = 2,
                    DriverCarId = 1,
                    RenterId = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",                 
                },
                new CarRoute()
                {
                    CarRouteId = 33,
                    Title ="Shared with One",
                    PickUpAddress = "Bulgaria, Sofia, Bul, Alexander malinov, 78",
                    DeliveryAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                    Description = "Whether you want a tourist tour from Sofia to Plovdiv, or simply busyness trip, this trip is private with a luxury limousine",
                    ImageUrlRouteGoogleMaps ="https://bulgaria-infoguide.com/wp-content/uploads/2018/10/green-taxi-1024x768.jpg",
                    Price = 158.40M,
                    CategoryId = 2,
                    DriverCarId = 1,
                    RenterId = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",           
                },
                new CarRoute()
                {
                    CarRouteId = 44,
                    Title ="OneWayLocal",
                    PickUpAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                    DeliveryAddress = "Antique Theater, str. Tsar Ivaylo 4, Plovdiv, Bulgaria",
                    Description = "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisy your expectation with a luxury limousine",
                    ImageUrlRouteGoogleMaps ="https://ekotaxi.bg/wp-content/uploads/2020/03/single_cab_redone-min-1-2048x1536.png",
                    Price = 6.20M,
                    CategoryId = 3,
                    DriverCarId = 1,
                    RenterId = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",                 
                },
                new CarRoute()
                {
                    CarRouteId = 55,
                    Title ="RoundTripLocal",
                    PickUpAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                    DeliveryAddress = "Antique Theater, str. Tsar Ivaylo 4, Plovdiv, Bulgaria",
                    Description = "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisfy your expectation with a luxury limousine",
                    ImageUrlRouteGoogleMaps ="https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600",
                    Price = 10.20M,
                    CategoryId = 4,
                    DriverCarId = 1,
                    RenterId = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",                 
                },
                new CarRoute()
                {
                    CarRouteId = 66,
                    Title ="Charter",
                    PickUpAddress = "Krumovo 4009, Rodopi Municipality, Plovdiv District",
                    DeliveryAddress = "Hartmann Road, London E16 2PX",
                    Description = "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisfy your expectation with a luxury limousine",
                    ImageUrlRouteGoogleMaps ="https://th.bing.com/th/id/R.4f634d4c26e3f1a1cda6459f649713d1?rik=GYIFZQe3lUWPJA&pid=ImgRaw&r=0",
                    Price = 10.20M,
                    CategoryId = 6,
                    DriverCarId = 1,
                    RenterId = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                },
            };
            return carRoutes;
        }
    }
}
