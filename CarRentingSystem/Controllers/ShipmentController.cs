using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Extensions;
using CarRentingSystem.Core.Models.Shipment;

using CarRentingSystem.Extensions;
using CarRentingSystem.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Controllers
{
    [Authorize]
    public class ShipmentController : Controller
    {
       
        private readonly IShipmentService shipmentService;

        private readonly IDriverService driverService;

        private readonly ILogger logger;

        public ShipmentController(
            IShipmentService _shipmentService,
            IDriverService _driverService,
            ILogger<ShipmentController> _logger)
        {
            shipmentService = _shipmentService;
            driverService = _driverService;
            logger = _logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllShipmentsQueryModel query)
        {
            var result = await shipmentService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllShipmentsQueryModel.ShipmentsPerPage);

            query.TotalShipmentsCount = result.TotalShipmentCount;
            query.Categories = await shipmentService.AllCategoriesNames();
            query.Shipments = result.Shipments;

            return View(query);
        }
      
        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole(AdminRolleName))
            {
                return RedirectToAction("Mine", "Shipment", new { area = AreaName });
            }

            IEnumerable<ShipmentServiceModel> myShipments;
            var userId = User.Id();

            if (await driverService.ExistsById(userId))
            {
                int driverId = await driverService.GetDriverId(userId);
                myShipments = await shipmentService.AllShipmentsByDriverId(driverId);
            }
            else
            {
                myShipments = await shipmentService.AllShipmentsByUserId(userId);
            }

            return View(myShipments);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int shipmentId, string information)
        {
            if ((await shipmentService.Exists(shipmentId)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            var model = await shipmentService.ShipmentDetailsByShipmentId(shipmentId);

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
            if ((await driverService.ExistsById(User.Id()) == false))
            {
                return RedirectToAction(nameof(DriverController.Become), "Driver");
            }

            var model = new ShipmentModel()
            {
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ShipmentModel model)
        {
            if ((await driverService.ExistsById(User.Id()) == false))
            {
                return RedirectToAction(nameof(DriverController.Become), "Driver");
            }

            if ((await shipmentService.CategoryExists(model.CategId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            int driverId = await driverService.GetDriverId(User.Id());

            int id = await shipmentService.Create(model, driverId);

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false)
            {
                logger.LogInformation("User with id {0} attempted to open other Driver house", User.Id());

                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var shipment = await shipmentService.ShipmentDetailsByShipmentId(id);
            var categoryId = await shipmentService.GetShipmentCategoryId(id);

            var model = new ShipmentModel()
            {
                Id = shipment.ShipmentId,
                LoadingAddress = shipment.LoadingAddress,
                DeliveryAddress = shipment.DeliveryAddress,
                CategId = categoryId,
                Description = shipment.Description,
                Price = shipment.Price,
                ImageUrlShipmentGoogleMaps = shipment.ImageUrlShipmentGoogleMaps,
                Title = shipment.Title,
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ShipmentModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await shipmentService.Exists(model.Id)) == false)
            {
                ModelState.AddModelError("", "Shipment not available");
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            if ((await shipmentService.HasDriverWithId(model.Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await shipmentService.CategoryExists(model.CategId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategId), "Category does not exist");
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            if (ModelState.IsValid == false)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            await shipmentService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { id = model.Id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var Shipment = await shipmentService.ShipmentDetailsByShipmentId(id);

            var model = new ShipmentDetailsViewModel()
            {
                LoadingAddress = Shipment.LoadingAddress,
                DeliveryAddress = Shipment.DeliveryAddress,
                Title = Shipment.Title,
                ImageUrlShipmentGoogleMapsUrl = Shipment.ImageUrlShipmentGoogleMaps
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, ShipmentDetailsViewModel model)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await shipmentService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRolleName) && await shipmentService.Exists(id))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (await shipmentService.IsRented(id))
            {
                return RedirectToAction(nameof(All));
            }

            await shipmentService.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if ((await shipmentService.Exists(id)) == false ||
                (await shipmentService.IsRented(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.IsRentedByUserWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await shipmentService.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}

