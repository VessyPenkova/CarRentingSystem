using CarRentingSystem.Core.Contracts.Users;
using CarRentingSystem.Core.Models.Drivers;
using CarRentingSystem.Core.Models.Users;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Infrastucture.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentingSystem.Core.Services.Users
{
    public class UserService : IUserService
    {

        //private readonly UserManager<User> userManager;
       // private readonly RoleManager<IdentityRole> roleManager;

        private readonly IRepository repo;



        public UserService(IRepository _repo)
        {
            repo = _repo;
        
        }

        public bool UserHasRents(string userId)

             => repo.AllReadonly<Shipment>()
                .Any(sh => sh.RenterId == userId);

        public async Task<string> UserFullName(string userId)
        {
            var user = await repo.GetByIdAsync<User>(userId);

            if (string.IsNullOrEmpty(user.FirstName)
                || string.IsNullOrEmpty(user.LastName))
            {
                return null;
            }
            else { 
            
            }

            return $"{user?.FirstName}+{user?.LastName}".Trim();
        }

        public async Task<IEnumerable<UserServiceModel>> All()
        {
            var allUsers = new List<UserServiceModel>();

            var users = await repo.AllReadonly<User>()
            .Select(u => new UserServiceModel()
            {
                Email = u.Email,
                FullName = $"{u.FirstName}+{u.LastName}".Trim(),
                PhoneNumber = u.PhoneNumber
            })
            .ToListAsync();

            allUsers.AddRange(users);

            var drivers = await repo.AllReadonly<Driver>()
              .Where(dr =>dr.UserId.Contains(dr.UserId) == false)
               .Select(dr => new UserServiceModel()
               {
                   
                   Email = dr.PhoneNumber,
               }).
               ToListAsync();

            allUsers.AddRange(drivers);

            return users;
        }
    }
}
