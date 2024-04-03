using CarRentingSystem.Areas.Admin.Models;
using CarRentingSystem.Core.Contracts.Drivers;
using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    public class ShipmentsController : AdminController
    {
        private readonly IShipmentService shipmentService;

        private readonly IDriverService driverService;

        public ShipmentsController(
            IShipmentService _shipmentService,
            IDriverService _driverService)
        {
            shipmentService = _shipmentService;
            driverService = _driverService;
        }

        public async Task<IActionResult> Mine()
        {
            var myShipments = new MyShipmentsViewModel();

            var adminId = User.Id();
            myShipments.RentedShipments = await shipmentService.AllShipmentsByUserId(adminId);
            var driverId = await driverService.GetDriverId(adminId);

            myShipments.AddedShipments = await shipmentService.AllShipmentsByDriverId(driverId);

            return View(myShipments);
        }
    }
}
