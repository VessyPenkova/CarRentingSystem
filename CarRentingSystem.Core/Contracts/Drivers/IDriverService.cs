namespace CarRentingSystem.Core.Contracts.Drivers
{
    public interface IDriverService
    {
        Task<bool> DriverWithPhoneNumberExists(string phoneNumber);

        Task<int> GetDriverId(string userId);

        Task<bool> ExistsById(string userId);


        Task Create(string userId, string phoneNumber);

    }
}
