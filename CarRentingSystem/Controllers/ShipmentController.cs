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


namespace CarRentingSystem.Controllers
{
    [Authorize]
    public class ShipmentController : Controller
    {
       
        private readonly IShipmentService shipmentService;

        private readonly IDriverService driverService;

        private readonly ILogger logger;

        private readonly IMemoryCache cache;

        public ShipmentController(
            IShipmentService _shipmentService,
            IDriverService _driverService,
            ILogger<ShipmentController> _logger,
            IMemoryCache cache)
        {
            shipmentService = _shipmentService;
            driverService = _driverService;
            logger = _logger;
            this.cache = cache;
        }

        [Authorize]
        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction(actionName: "Mine", controllerName: "Shipment", new { area = "Admin" });
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
            query.Shipments = result.Shipments;

            var shipmentCategories = await shipmentService.AllCategoriesNames();

            return View(query);
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
                return BadRequest();
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if ((await driverService.ExistsById(User.Id()) == false))
            {
                return RedirectToAction(nameof(DriverController.Become), "Driver");
            }

            var model = new ShipmentFormModel()
            {
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(ShipmentFormModel model)
        {
            if ((await driverService.ExistsById(User.Id()) == false))
            {
                return RedirectToAction(nameof(DriverController.Become), "Driver");
            }

            if ((await shipmentService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            var driverId = await driverService.GetDriverId(User.Id());

            model = new ShipmentFormModel()
            {
                ShipmentCategories = await shipmentService.AllCategories()
            };

            TempData["message"] = "You have created new shipment!";

            return RedirectToAction(nameof(Details), 
            new { id = model.Id, information = model.GetInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                TempData["message"] = "This shipment already exist!";
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false)
            {
                logger.LogInformation("User with id {0} attempted to open other Driver shipment", User.Id());

                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var shipment = await shipmentService.ShipmentDetailsByShipmentId(id);

            var categoryId = await shipmentService.GetShipmentCategoryId(shipment.ShipmentId);

            var model = new ShipmentCreateEditFormModel()
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
        [Authorize]
        public async Task<IActionResult> Edit(int id, ShipmentCreateEditFormModel model)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                ModelState.AddModelError("", "Shipment not available");
                model.ShipmentCategories = await shipmentService.AllCategories();

                return View(model);
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false && !User.IsInRole("Admin"))
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

            await shipmentService.Edit(model.Id, model);

            TempData["message"] = "Details updated!";

            return RedirectToAction(nameof(Details), new { id = model.Id, information = model.GetInformation() });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                TempData["message"] = "This shipment is not available!";
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false && !User.IsInRole("Admin"))
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
        [HttpPost]
        public async Task<IActionResult> Delete(int id, ShipmentDetailsViewModel model)
        {
            if ((await shipmentService.Exists(id)) == false)
            {
                TempData["message"] = "This shipment is not available!";
                return RedirectToAction(nameof(All));
            }

            if ((await shipmentService.HasDriverWithId(id, User.Id())) == false && !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await shipmentService.Delete(id);

            TempData["message"] = "You have successfully removed this shipment!";

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Rent(int id)
        {
            if ((await shipmentService.Exists(id)) == false)
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
        [Authorize]
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

            this.cache.Remove(RentsCacheKey);

            TempData["message"] = "You have successfully left the car!";

            return RedirectToAction(nameof(Mine));
        }
    }
}

