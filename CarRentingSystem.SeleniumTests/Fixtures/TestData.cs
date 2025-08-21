namespace CarRentingSystem.SeleniumTests.Utils;

public static class TestData
{
    public static string UniqueEmail() => $"ui_{Guid.NewGuid():N}@example.test";
    public static string StrongPassword() => "Passw0rd!123";
    public static string UniqueTitle(string prefix = "Shipment") =>
        $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N[..6]}";

    // Seed assumes categories exist (1..N). Use 1 by default.
    public const string DefaultCategoryId = "1";
}
