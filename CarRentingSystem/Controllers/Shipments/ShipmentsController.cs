using CarRentingSystem.Controllers.Drivers;
using CarRentingSystem.Core.Contracts.Drivers;
using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Core.Extensions;
using CarRentingSystem.Core.Models.Shipment;
using CarRentingSystem.Core.Services.Drivers;
using CarRentingSystem.Core.Services.Shipments;
using CarRentingSystem.Extensions;
using CarRentingSystem.Infrastucture.Data;
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
        private readonly ILogger<ShipmentsController> logger;
        private readonly IMemoryCache cache;

        public ShipmentsController(
            IShipmentService shipmentService,
            IDriverService driverService,
            ILogger<ShipmentsController> logger,
            IMemoryCache cache)
        {
            this.shipmentService = shipmentService;
            this.driverService = driverService;
            this.logger = logger;
            this.cache = cache;
        }

        [HttpGet, AllowAnonymous]
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // If you REQUIRE only non-drivers to create shipments, remove this block.
            // If you REQUIRE drivers to create shipments, keep the redirect:
            // if (!await driverService.ExistsById(User.Id()))
            //     return RedirectToAction(nameof(DriversController.Become), "Drivers");

            var model = new ShipmentCoreFormModel
            {
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ShipmentCoreFormModel model)
        {
            if (!await shipmentService.CategoryExists(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
            }

            if (!ModelState.IsValid)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();
                return View(model);
            }

            // Creator is the current user; no driver assignment at create time
            int newShipmentId = await shipmentService.Create(
                model.Title,
                model.LoadingAddress,
                model.DeliveryAddress,
                model.Description,
                model.ImageUrlShipmentGoogleMaps,
                model.Price,
                model.CategoryId,
                driverId: null,
                creatorUserId: User.Id());

            TempData["message"] = "You have created new shipment!";

            return RedirectToAction(nameof(Details), new
            {
                id = newShipmentId,
                information = model.GetInformation()
            });
        }







        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, string information)
        {
            if (!await shipmentService.Exists(id))
            {
                return RedirectToAction(nameof(All));
            }

            var model = await shipmentService.ShipmentDetailsByShipmentId(id);

            if (!string.Equals(information, model.GetInformation(), System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Don't touch!";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await shipmentService.Exists(id))
            {
                TempData["message"] = "This shipment does not exist!";
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRoleName) &&
                !await shipmentService.IsOwner(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var sh = await shipmentService.ShipmentDetailsByShipmentId(id);
            var categoryId = await shipmentService.GetShipmentCategoryId(id);

            var model = new ShipmentCoreFormModel
            {
                Id = sh.ShipmentId,
                Title = sh.Title,
                LoadingAddress = sh.LoadingAddress,
                DeliveryAddress = sh.DeliveryAddress,
                Description = sh.Description,
                Price = sh.Price,
                ImageUrlShipmentGoogleMaps = sh.ImageUrlShipmentGoogleMaps,
                CategoryId = categoryId,
                ShipmentCategories = await shipmentService.AllCategories()
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShipmentCoreFormModel model)
        {
            if (id != model.Id)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (!await shipmentService.Exists(id))
            {
                ModelState.AddModelError("", "Shipment does not exist");
                model.ShipmentCategories = await shipmentService.AllCategories();
                return View(model);
            }

            if (!User.IsInRole(AdminRoleName) &&
                !await shipmentService.IsOwner(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (!await shipmentService.CategoryExists(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
                model.ShipmentCategories = await shipmentService.AllCategories();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.ShipmentCategories = await shipmentService.AllCategories();
                return View(model);
            }

            await shipmentService.Edit(id, model.Title, model.LoadingAddress, model.DeliveryAddress,
                model.Description, model.ImageUrlShipmentGoogleMaps, model.Price, model.CategoryId);

            TempData["message"] = "Details updated!";

            return RedirectToAction(nameof(Details), new { id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await shipmentService.Exists(id))
            {
                TempData["message"] = "This shipment is not available!";
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRoleName) &&
                !await shipmentService.IsOwner(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var sh = await shipmentService.ShipmentDetailsByShipmentId(id);

            var model = new ShipmentDetailsViewModel
            {
                Title = sh.Title,
                LoadingAddress = sh.LoadingAddress,
                DeliveryAddress = sh.DeliveryAddress,
                ImageUrlShipmentGoogleMapsUrl = sh.ImageUrlShipmentGoogleMaps
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, ShipmentDetailsViewModel _)
        {
            if (!await shipmentService.Exists(id))
            {
                TempData["message"] = "This shipment is not available!";
                return RedirectToAction(nameof(All));
            }

            if (!User.IsInRole(AdminRoleName) &&
                !await shipmentService.IsOwner(id, User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await shipmentService.Delete(id);

            TempData["message"] = "You have successfully removed this shipment!";
            return RedirectToAction(nameof(All));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(int id)
        {
            if (!await shipmentService.Exists(id))
                return RedirectToAction(nameof(All));

            if (await shipmentService.IsOwner(id, User.Id()))
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (await shipmentService.IsRented(id))
                return RedirectToAction(nameof(All));

            await shipmentService.Rent(id, User.Id());
            return RedirectToAction(nameof(Mine));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(int id)
        {
            if (!await shipmentService.Exists(id) || !await shipmentService.IsRented(id))
                return RedirectToAction(nameof(All));

            if (!await shipmentService.IsRentedByUserWithId(id, User.Id()))
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            await shipmentService.Leave(id);
            TempData["message"] = "You have successfully left the car!";
            return RedirectToAction(nameof(Mine));
        }


        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction(nameof(All));
            }

            IEnumerable<ShipmentServiceModel> myShipments;
            var userId = User.Id();

            if (await driverService.ExistsById(userId))
            {
                var currentDriverId = await driverService.GetDriverId(userId);
                myShipments = await shipmentService.AllShipmentsByDriverId(currentDriverId);
            }
            else
            {
                myShipments = await shipmentService.AllShipmentsByUserId(userId);
            }

            return View(myShipments);
        }


        //[HttpGet]
        //public async Task<IActionResult> MyShipments()
        //{
        //    if (!User.IsInRole(DriverRoleName))
        //        return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
        //    var driverId = await driverService.GetDriverIdByUserId(User.Id());
        //    var shipments = await shipmentService.AllShipmentsByDriverId(driverId);
        //    return View(shipments);
        //}

    }
}



    
