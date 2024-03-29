using CarRentingSystem.Core.Models.Users;

namespace CarRentingSystem.Core.Contracts.Users
{
    public interface IUserService
    {
        Task<string> UserFullName(string userId);

        Task<IEnumerable<UserServiceModel>> All();

        bool UserHasRents(string userId);
    }
}
