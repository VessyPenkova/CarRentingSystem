using CarRentingSystem.Core.Contracts.Users;
using CarRentingSystem.Core.Models.Users;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;
        private readonly UserManager<User> userManager;
        public UserService(IRepository _repo, UserManager<User> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }
        public bool UserHasRents(string userId)

             => repo.AllReadonly<Shipment>()
                .Any(sh => sh.RenterId == userId);

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<IdentityUser>(userId);

            return $"{user?.UserName}".Trim();
        }

        public async Task<IEnumerable<UserServiceModel>> All()
        {
            var allUsers = new List<UserServiceModel>();

            var drivers = await repo.AllReadonly<Driver>()
            .Where(dr => dr.User.Id != null)
            .Select(dr => new UserServiceModel()
            {
                UserId = dr.UserId,
                Email = dr.User.Email,
                FullName = dr.User.UserName,
                PhoneNumber = dr.PhoneNumber
            })
            .ToListAsync();

            allUsers.AddRange(drivers);

            var users = await repo.AllReadonly<User>()
            .Select(u => new UserServiceModel()
            {
                UserId = u.Id,
                Email = u.Email,
                FullName = u.UserName
            })
            .ToListAsync();

            allUsers.AddRange(users);

            return users;
        }
    }
}