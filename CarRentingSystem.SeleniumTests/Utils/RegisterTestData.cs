namespace CarRentingSystem.SeleniumTests.TestData
{
    public static class RegisterTestData
    {
        public sealed record Model(string Email, string FirstName, string LastName, string Password, string ConfirmPassword);

        public static Model Valid() =>
            new Model(
                Email: $"ui_{Guid.NewGuid():N}@example.test",
                Password: "Passw0rd!123",
                ConfirmPassword: "Passw0rd!123",
                FirstName: "Test",
                LastName: "User"
               
            );
    }
}
