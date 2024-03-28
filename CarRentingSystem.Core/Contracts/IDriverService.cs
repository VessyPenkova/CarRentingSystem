namespace CarRentingSystem.Core.Contracts
{
    public interface IDriverService
    {
        Task<bool> DriverWithPhoneNumberExists(string phoneNumber);

        Task<bool> ExistsById(string userId);

        Task<int> GetDriverId(string userId);      

        Task Create(string userId, string phoneNumber);

    }
}
