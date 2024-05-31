using CarRentingSystem.Core.Contracts.Users;
using CarRentingSystem.Core.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IUserService userServices;
        private readonly IMemoryCache cache;

        public UsersController(IUserService _userServices, IMemoryCache _cache)
        {
            this.userServices = _userServices;
            this.cache = _cache;
        }

        [Route("Users/All")]
        [HttpPost]
        public async Task<IActionResult> All()
        {

            var users = this.cache
                .Get<IEnumerable<UserServiceModel>>(UsersCacheKey);

            if (users == null)
            {
                users = await this.userServices.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(UsersCacheKey, users, cacheOptions);
            }

            return View(users);
        }
    }
}