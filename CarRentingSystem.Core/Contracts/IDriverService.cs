namespace CarRentingSystem.Core.Contracts
{
    public interface IDriverService
    {
        Task<bool> ExistsById(string userId);

        Task<bool> UserWithPhoneNumberExists(string phoneNumber);

        Task<bool> UserHasRents(string userId);

        Task Create(string userId, string phoneNumber);

        Task<int> GetDriverId(string userId);
    }
}
