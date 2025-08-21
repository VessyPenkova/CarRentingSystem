using System;
namespace CarRentingSystem.SeleniumTests.Utils
{
    public static class ShipmentTestData
    {
        public sealed record Input(
            string Title,
            string LoadingAddress,
            string DeliveryAddress,
            string Description,
            string ImageUrlShipmentGoogleMaps,
            string Price,
            string CategoryId)
        {
            public Input With(
                string? title = null,
                string? loadingAddress = null,
                string? deliveryAddress = null,
                string? description = null,
                string? imageUrlShipmentGoogleMaps = null,
                string? price = null,
                string? categoryId = null)
                => new(
                    title ?? Title,
                    loadingAddress ?? LoadingAddress,
                    deliveryAddress ?? DeliveryAddress,
                    description ?? Description,
                    imageUrlShipmentGoogleMaps ?? ImageUrlShipmentGoogleMaps,
                    price ?? Price,
                    categoryId ?? CategoryId);
        }

        public const string DefaultCategoryId = "1"; // matches your seed

        public static string UniqueTitle(string prefix = "Shipment")
            => $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N[..6]}";

        // ---- Valid happy-path ----
        public static Input Valid(string? categoryId = null) => new(
            Title: UniqueTitle("UI"),
            LoadingAddress: "Paris",
            DeliveryAddress: "Plovdiv",
            Description: "UI test shipment",
            ImageUrlShipmentGoogleMaps: "https://picsum.photos/800/400",
            Price: "50.00",
            CategoryId: categoryId ?? DefaultCategoryId
        );

        // ---- Common negatives ----
        public static Input MissingTitle() => Valid().With(title: "");
        public static Input InvalidCategory() => Valid().With(categoryId: "99999");
        public static Input InvalidPrice() => Valid().With(price: "abc");
        public static Input BadImageUrl() => Valid().With(imageUrlShipmentGoogleMaps: "not-a-url");
        public static Input TooLongTitle()
            => Valid().With(title: new string('X', 300));
    }
}
