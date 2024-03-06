using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarRouteService carRouteService;       

        private readonly ILogger logger;

        public HomeController(
            ICarRouteService _carRouteService,           
            ILogger<HomeController> _logger)
        {
            carRouteService = _carRouteService;         
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
            var feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();

            logger.LogError(feature.Error, "TraceIdentifier: {0}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
