using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShipmentService shipmentService;       

        private readonly ILogger logger;

        public HomeController(
            IShipmentService _shipmentService,           
            ILogger<HomeController> _logger)
        {
            shipmentService = _shipmentService;         
            logger = _logger;
        }
        public async Task< IActionResult> Index()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            var collectionOfLastShipmentsModel = await shipmentService.LastThreeShipments();

            return View(collectionOfLastShipmentsModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();

            logger.LogError(feature.Error, "TraceIdentifier: {0}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


