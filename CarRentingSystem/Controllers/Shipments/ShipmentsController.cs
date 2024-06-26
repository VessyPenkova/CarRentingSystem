﻿using CarRentingSystem.Controllers.Drivers;
using CarRentingSystem.Core.Contracts.Drivers;
using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Core.Extensions;
using CarRentingSystem.Core.Models.Shipment;

using CarRentingSystem.Extensions;
using CarRentingSystem.Models.Shipments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;


namespace CarRentingSystem.Controllers.Shipments
{
    [Authorize]
    public class ShipmentsController : Controller
    {

        private readonly IShipmentService shipmentService;

        private readonly IDriverService driverService;

        private readonly ILogger logger;

        private readonly IMemoryCache cache;

        public ShipmentsController(
            IShipmentService _shipmentService,
            IDriverService _driverService,
            ILogger<ShipmentsController> _logger,
            IMemoryCache cache)
        {
            shipmentService = _shipmentService;
            driverService = _driverService;
            logger = _logger;
            this.cache = cache;
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
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction(actionName: "Mine", controllerName: "Shipment", new { area = "AreaName" });
            }

            IEnumerable<ShipmentServiceModel> myShipments;

            var userId = User.Id();

            if (await driverService.ExistsById(userId))
            {
                int currentDriverId = await driverService.GetDriverId(userId);

                myShipments = await shipmentService.AllShipmentsByDriverId(currentDriverId);
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
            if (await shipmentService.Exists(shipmentId) == false)
            {
                return RedirectToAction(nameof(All));
            }

            var model = await shipmentService.ShipmentDetailsByShipmentId(shipmentId);

            if (information != model.GetInformation())
            {
                TempData["ErrorMessage"] = "Don't touch!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (await driverService.ExistsById(User.Id()) == false)
            {
                return RedirectToAction(nameof(DriversController.Become), "Drivers");
            }

            var model = new Core.Models.Shipment.ShipmentCoreFormModel()
            {
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Add(ShipmentCoreFormModel model)
        {
            if (await driverService.ExistsById(User.Id()) == false)
            {
                return RedirectToAction(nameof(DriversController.Become), "Drivers");
            }

            if (await shipmentService.CategoryExists(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            var driverId = await driverService.GetDriverId(User.Id());

            var newShipmentId = shipmentService.Create(model.Title, model.LoadingAddress, model.DeliveryAddress,
               model.Description, model.ImageUrlShipmentGoogleMaps, model.Price,
            model.CategoryId, driverId);

            TempData["message"] = "You have created new shipment!";

            return RedirectToAction(nameof(Details),
            new { id = driverId, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await shipmentService.Exists(id) == false)
            {
                TempData["message"] = "This shipment already exist!";
                return RedirectToAction(nameof(All));
            }

            if (await shipmentService.HasDriverWithId(id, User.Id()) == false)
            {
                logger.LogInformation("User with id {0} attempted to open other Driver shipment", User.Id());

                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var shipment = await shipmentService.ShipmentDetailsByShipmentId(id);

            var categoryId = await shipmentService.GetShipmentCategoryId(id);

            var model = new Core.Models.Shipment.ShipmentCoreFormModel()
            {
                Id = shipment.ShipmentId,
                LoadingAddress = shipment.LoadingAddress,
                DeliveryAddress = shipment.DeliveryAddress,
                CategoryId = categoryId,
                Description = shipment.Description,
                Price = shipment.Price,
                ImageUrlShipmentGoogleMaps = shipment.ImageUrlShipmentGoogleMaps,
                Title = shipment.Title,
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ShipmentCoreFormModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }


            if (await shipmentService.Exists(id) == false)
            {
                ModelState.AddModelError("", "Shipment not exist");
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            if (await shipmentService.HasDriverWithId(model.Id, User.Id()) == false )
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await shipmentService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            if (ModelState.IsValid == false)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            await shipmentService.Edit(id, model.Title, model.LoadingAddress, model.DeliveryAddress,
                model.Description, model.ImageUrlShipmentGoogleMaps, model.Price, model.CategoryId);

            TempData["message"] = "Details updated!";

            return RedirectToAction(nameof(Details), new { id = model.Id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await shipmentService.Exists(id) == false)
            {
                TempData["message"] = "This shipment is not available!";
                return RedirectToAction(nameof(All));
            }

            if (await shipmentService.HasDriverWithId(id, User.Id()) == false && !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var shipment = await shipmentService.ShipmentDetailsByShipmentId(id);

            var model = new ShipmentDetailsViewModel()
            {
                LoadingAddress = shipment.LoadingAddress,
                DeliveryAddress = shipment.DeliveryAddress,
                Title = shipment.Title,
                ImageUrlShipmentGoogleMapsUrl = shipment.ImageUrlShipmentGoogleMaps
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id, ShipmentDetailsViewModel model)
        {
            if (await shipmentService.Exists(id) == false)
            {
                TempData["message"] = "This shipment is not available!";
                return RedirectToAction(nameof(All));
            }

            if (await shipmentService.HasDriverWithId(id, User.Id()) == false && !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await shipmentService.Delete(id);

            TempData["message"] = "You have successfully removed this shipment!";

            return RedirectToAction(nameof(All));
        }

        [HttpPost]

        public async Task<IActionResult> Rent(int id)
        {
            if (await shipmentService.Exists(id) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRoleName))
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
            if (await shipmentService.Exists(id) == false ||
                await shipmentService.IsRented(id) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if (await shipmentService.IsRentedByUserWithId(id, User.Id()) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await shipmentService.Leave(id);

            cache.Remove(RentsCacheKey);

            TempData["message"] = "You have successfully left the car!";

            return RedirectToAction(nameof(Mine));
        }
    }
}

