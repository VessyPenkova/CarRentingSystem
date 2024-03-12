using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Extensions;
using CarRentingSystem.Core.Models.CarRoute;
using CarRentingSystem.Core.Models.DriverCar;
using CarRentingSystem.Core.Services;
using CarRentingSystem.Extensions;
using CarRentingSystem.Infrastucture.Data;
using CarRentingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Controllers
{
    [Authorize]
    public class CarRouteController : Controller
    {
       
        private readonly ICarRouteService carRouteService;

        private readonly ICarDriverService driverCarService;

        private readonly ILogger logger;

        public CarRouteController(
            ICarRouteService _carRouteService,
            ICarDriverService _driverCarService,
            ILogger<CarRouteController> _logger)
        {
            carRouteService = _carRouteService;
            driverCarService = _driverCarService;
            logger = _logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllCarRoutesQueryModel query)
        {
            var result = await carRouteService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarRoutesQueryModel.CarRoutesPerPage);

            query.TotalCarRoutesCount = result.TotalCarRouteCount;
            query.Categories = await carRouteService.AllCategoriesNames();
            query.CarRoutes = result.CarRoutes;

            return View(query);
        }

       
        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole(AdminRolleName))
            {
                return RedirectToAction("Mine", "CarRoute", new { area = AreaName });
            }

            IEnumerable<CarRouteServiceModel> myCarRoutes;
            var userId = User.Id;

            if (await driverCarService.ExistsById(User.Id()))
            {
                int driverId = await driverCarService.GetDriverId(User.Id());
                myCarRoutes = await carRouteService.AllCarRoutesByDriverId(driverId);
            }
            else
            {
                myCarRoutes = await carRouteService.AllRoutesByUserId(User.Id());
            }

            return View(myCarRoutes);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string carRouteId, string information)
        {
            if ((await driverCarService.ExistsById(carRouteId)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            var model = await carRouteService.CarRouteDetailsByCarRouteId(int.Parse(carRouteId));

            if (information != model.GetInformation())
            {
                TempData["ErrorMessage"] = "Don't touch my slug!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if ((await driverCarService.ExistsById(User.Id()) == false))
            {
                return RedirectToAction(nameof(CarDriverController.Become), "CarDriver");
            }

            var model = new CarRouteModel()
            {
                CarRouteCategories = await carRouteService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarRouteModel model)
        {
            if ((await driverCarService.ExistsById(User.Id()) == false))
            {
                return RedirectToAction(nameof(CarDriverController.Become), "CarDriver");
            }

            if ((await carRouteService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.CarRouteCategories = await carRouteService.AllCategories();

                return View(model);
            }

            int carDriverId = await driverCarService.GetDriverId(User.Id());

            int id = await carRouteService.Create(model, carDriverId);

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if ((await carRouteService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await carRouteService.HasDriverCarWithId(id, User.Id())) == false)
            {
                logger.LogInformation("User with id {0} attempted to open other agent house", User.Id());

                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var carRoute = await carRouteService.CarRouteDetailsByCarRouteId(id);
            var categoryId = await carRouteService.GetRouteCategoryId(id);

            var model = new CarRouteModel()
            {
                Id = carRoute.CarRouteId,
                PickUpAddress = carRoute.PickUpAddress,
                DeliveryAddress = carRoute.DeliveryAddress,
                Description = carRoute.Description,
                Price = carRoute.Price,
                ImageUrlRouteGoogleMaps = carRoute.ImageUrlRouteGoogleMaps,
                Title = carRoute.Title,
                CarRouteCategories = await carRouteService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarRouteModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await carRouteService.Exists(model.Id)) == false)
            {
                ModelState.AddModelError("", "House does not exist");
                model.CarRouteCategories = await carRouteService.AllCategories();

                return View(model);
            }

            if ((await carRouteService.HasDriverCarWithId(model.Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await carRouteService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
                model.CarRouteCategories = await carRouteService.AllCategories();

                return View(model);
            }

            if (ModelState.IsValid == false)
            {
                model.CarRouteCategories = await carRouteService.AllCategories();

                return View(model);
            }

            await carRouteService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { id = model.Id, information = model.GetInformation() });
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if ((await carRouteService.Exists(id)) == false)
        //    {
        //        return RedirectToAction(nameof(All));
        //    }

        //    if ((await carRouteService.HasDriverCarWithId(id, User.Id())) == false)
        //    {
        //        return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
        //    }

        //    var carRoute = await carRouteService.CarRouteDetailsByCarRouteId(id);
        //    var model = new CarRouteDetailsViewModel()
        //    {
        //        PickUpAddress = carRoute.PickUpAddress,
        //        DeliveryAddress = carRoute.DeliveryAddress,
        //        Title = carRoute.Title,
        //        ImageImageUrlRouteGoogleMapsUrl = carRoute.ImageUrlRouteGoogleMaps
        //    };

        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> Delete(int id, CarRouteDetailsViewModel model)
        {
            if ((await carRouteService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await carRouteService.HasDriverCarWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await carRouteService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if ((await carRouteService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRolleName) && await carRouteService.Exists(id))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (await carRouteService.IsRented(id))
            {
                return RedirectToAction(nameof(All));
            }

            await carRouteService.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if ((await carRouteService.Exists(id)) == false ||
                (await carRouteService.IsRented(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await carRouteService.IsRentedByUserWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await carRouteService.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}

