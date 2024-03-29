using CarRentingSystem.Core.Constants;
using CarRentingSystem.Core.Contracts.Drivers;
using CarRentingSystem.Core.Contracts.Users;
using CarRentingSystem.Core.Models.Drivers;
using CarRentingSystem.Extensions;
using CarRentingSystem.Models.Drivers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Controllers
{
    [Authorize]
    public class DriverController : Controller
    {
        private readonly IDriverService driverService;
        private readonly IUserService usersService;
        public DriverController(IDriverService _driverService,  IUserService _usersService)
        {
            driverService = _driverService;
            usersService = _usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            if (await driverService.ExistsById(User.Id()))
            {
                TempData[MessageConstant.ErrorMessage] = "You are already registered as driver";

                return RedirectToAction("Index", "Home");
            }

            var model = new BecomeDriverFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeDriverFormModel model)
        {
            var userId = User.Id();

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (usersService.UserHasRents(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "You can not have booked shipments to become driver";

                return RedirectToAction("Index", "Home");
            }

            if (await driverService.ExistsById(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "Phone number already exists. Enter another one.";

                return RedirectToAction("Index", "Home");
            }

            if (await driverService.DriverWithPhoneNumberExists(model.PhoneNumber))
            {
                TempData[MessageConstant.ErrorMessage] = "The phone already exist";

                return RedirectToAction("Index", "Home");
            }


            await driverService.Create(userId, model.PhoneNumber);

            return RedirectToAction(nameof(ShipmentController.All), "Shipments");
        }
    }
}
