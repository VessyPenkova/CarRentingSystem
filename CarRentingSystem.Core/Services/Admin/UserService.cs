using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Contracts.Admin;
using CarRentingSystem.Core.Models.Admin;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IRepository _repo, UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }
        public async Task<IEnumerable<UserServiceModel>> All()
        {
            List<UserServiceModel> result;
            result = await repo.AllReadonly<DriverCar>()
            .Where(dc => (bool)dc.User.IsActive)
            .Select(dc => new UserServiceModel()
            {
                UserId = dc.UserId,
                Email = dc.User.Email,
                FullName = $"{dc.User.FirstName} {dc.User.LastName}",
                PhoneNumber = dc.PhoneNumber
            })
            .ToListAsync();
            string[] driverCarIds = result.Select(dc => dc.UserId).ToArray();

            result.AddRange(await repo.AllReadonly<ApplicationUser>()
               .Where(u => driverCarIds.Contains(u.Id) == false)
               .Where(u => (bool)u.IsActive)
               .Select(u => new UserServiceModel()
               {
                   UserId = u.Id,
                   Email = u.Email,
                   FullName = $"{u.FirstName} {u.LastName}"
               }).ToListAsync());

            return result;
        }

        public async Task<bool> Forget(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.PhoneNumber = null;
            user.FirstName = null;
            user.Email = null;
            user.IsActive = false;
            user.LastName = null;
            user.NormalizedEmail = null;
            user.NormalizedUserName = null;
            user.PasswordHash = null;
            user.UserName = $"forgottenUser-{DateTime.Now.Ticks}";

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            return $"{user?.FirstName} {user?.LastName}".Trim();
        }
    }
}
