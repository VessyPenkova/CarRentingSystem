using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Extensions;
using CarRentingSystem.Core.Services;
using CarRentingSystem.Core.Models.DriverCar;

using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;
using CarRentingSystem.Core.Constants;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Extensions;

namespace CarRentingSystem.Controllers
{
    [Authorize]
    public class CarDriverController : Controller
    {
        private readonly ICarDriverService carDriverService;

        private readonly ICarRouteService carRouteService;

        private readonly ILogger logger;

        public CarDriverController(ICarDriverService _carDriverService ,
            ICarRouteService _carRouteService, ILogger _logger)
        {
            carDriverService = _carDriverService;
            carRouteService = _carRouteService;
            logger = _logger;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            if (await carDriverService.ExistsById(User.Id()))
            {
                TempData[MessageConstant.ErrorMessage] = "You are already registered";

                return RedirectToAction("Index", "Home");
            }

            var model = new BecomeDriverCarModel();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Become(BecomeDriverCarModel model)
        {
            var userId = User.Id();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await carDriverService.ExistsById(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "You are already registered";

                return RedirectToAction("Index", "Home");
            }

            if (await carDriverService.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                TempData[MessageConstant.ErrorMessage] = "The phone already exist";

                return RedirectToAction("Index", "Home");
            }

            if (await carDriverService.UserHasRents(userId))
            {
                TempData[MessageConstant.ErrorMessage] = "You must not have a booking to become driver";

                return RedirectToAction("Index", "Home");
            }

            await carDriverService.Create(userId, model.PhoneNumber);

            return RedirectToAction("All", "CarRoute");
        }
    }
}
