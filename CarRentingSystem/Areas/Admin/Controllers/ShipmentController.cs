using CarRentingSystem.Areas.Admin.Models;
using CarRentingSystem.Core.Contracts.Drivers;
using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    public class ShipmentController : AdminController
    {
        private readonly IShipmentService ShipmentService;

        private readonly IDriverService DriverService;

        public ShipmentController(
            IShipmentService _ShipmentService,
            IDriverService _DriverService)
        {
            ShipmentService = _ShipmentService;
            DriverService = _DriverService;
        }

        public async Task<IActionResult> Mine()
        {
            var myShipments = new MyShipmentsViewModel();

            var adminId = User.Id();
            myShipments.RentedShipments = await ShipmentService.AllShipmentsByUserId(adminId);
            var driverId = await DriverService.GetDriverId(adminId);

            myShipments.AddedShipments = await ShipmentService.AllShipmentsByDriverId(driverId);

            return View(myShipments);
        }
    }
}
