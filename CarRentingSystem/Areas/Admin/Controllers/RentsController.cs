using CarRentingSystem.Core.Models.Shipment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;
using CarRentingSystem.Core.Contracts.Rents;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    public class RentsController : AdminController
    {
        private readonly IRentService rents;
        private readonly IMemoryCache cache;

        public RentsController(IRentService rents,
            IMemoryCache cache)
        {
            this.rents = rents;
            this.cache = cache;
        }



        [Route("Rents/All")]
        public async Task<IActionResult> All()
        {
            var rents = this.cache
                .Get<IEnumerable<RentServiceModel>>(RentsCacheKey);

            if (rents == null)
            {
                rents = await this.rents.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(RentsCacheKey, rents, cacheOptions);
            }

            return View(rents);
        }
    }
}
