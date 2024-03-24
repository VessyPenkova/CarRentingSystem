using CarRentingSystem.Core.Constants;
using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Models.Driver;
using CarRentingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Controllers
{
    [Authorize]
    public class DriverController : Controller
    {
        private readonly IDriverService DriverService;
        public DriverController(IDriverService _DriverService)
        {
            DriverService = _DriverService;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            if (await DriverService.ExistsById(User.Id()))
            {
                TempData[MessageConstant.ErrorMessage] = "You are already registered as driver";

                return RedirectToAction("Index", "Home");
            }

            var model = new BecomeDriverModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeDriverModel model)
        {
            var userId = User.Id();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await DriverService.ExistsById(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "You are already registered as driver";

                return RedirectToAction("Index", "Home");
            }

            if (await DriverService.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                TempData[MessageConstant.ErrorMessage] = "The phone already exist";

                return RedirectToAction("Index", "Home");
            }

            if (await DriverService.UserHasRents(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "You must not have a rents to become driver";

                return RedirectToAction("Index", "Home");
            }

            await DriverService.Create(userId, model.PhoneNumber);

            return RedirectToAction("All", "Shipment");
        }
    }
}
