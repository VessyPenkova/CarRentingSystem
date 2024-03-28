using CarRentingSystem.Core.Contracts.Admin;
using CarRentingSystem.Core.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        private readonly IUserService userServices;
        private readonly IMemoryCache cache;

        public UserController(IUserService _userServices,  IMemoryCache _cache)
        {
            this.userServices = _userServices;
            this.cache = _cache;
        }

     
        [Route("Users/All")]
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
