using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Core.Services;
using CarRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using CarRentingSystem.Areas.Admin.Constants;

namespace CarRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarRouteService carRouteService;

        private readonly ICarDriverService driverCarService;

        private readonly ILogger logger;

        public HomeController(
            ICarRouteService _carRouteService,
            ICarDriverService _driverCarService,
            ILogger<HomeController> _logger)
        {
            carRouteService = _carRouteService;
            driverCarService = _driverCarService;
            logger = _logger;
        }
        public async Task< IActionResult> Index()
        {
            if (User.IsInRole(AdminRolleName))
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            var model = await carRouteService.LastThreeRoutes();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
