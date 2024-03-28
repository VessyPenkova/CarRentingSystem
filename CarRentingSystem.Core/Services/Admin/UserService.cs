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

        private readonly UserManager<IdentityUser> userManager;

        public UserService(IRepository _repo, UserManager<IdentityUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }
        public async Task<IEnumerable<UserServiceModel>> All()
        {
            List<UserServiceModel> result;

            result = await repo.AllReadonly<Driver>()
            .Select(dc => new UserServiceModel()
            {
                UserId = dc.UserId,
                Email = dc.User.Email,
                FullName = dc.User.UserName,
                PhoneNumber = dc.PhoneNumber
            })
            .ToListAsync();

            string[] driverIds = result.Select(dc => dc.UserId).ToArray();

            result.AddRange(await repo.AllReadonly<IdentityUser>()
               .Where(u => driverIds.Contains(u.Id) == false)
               .Select(u => new UserServiceModel()
               {
                   UserId = u.Id,
                   Email = u.Email,
                   FullName = u.UserName
               }).ToListAsync());

            return  result;
        }

        public async Task<bool> UserHasRents(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.PhoneNumber = null;
            user.UserName = null;
            user.Email = null;
            user.NormalizedEmail = null;
            user.NormalizedUserName = null;
            user.PasswordHash = null;
            user.UserName = $"forgottenUser-{DateTime.Now.Ticks}";

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<IdentityUser>(userId);

            return $"{user?.UserName}".Trim();
        }
    }
}
