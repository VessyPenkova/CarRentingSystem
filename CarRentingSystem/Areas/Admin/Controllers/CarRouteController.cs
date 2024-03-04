using CarRentingSystem.Areas.Admin.Models;
using CarRentingSystem.Core.Contracts;
using CarRentingSystem.Extensions;
using CarRentingSystem.Infrastucture.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    public class CarRouteController : BaseController
    {
        private readonly ICarRouteService carRouteService;

        private readonly ICarDriverService carDriverService;

        public CarRouteController(
            ICarRouteService _carRouteService,
            ICarDriverService _carDriverService)
        {
            carRouteService = _carRouteService;
            carDriverService = _carDriverService;
        }

        public async Task<IActionResult> Mine()
        {
            var myCarRoutes = new MyCarRoutesViewModel();
            var adminId = User.Id();
            myCarRoutes.RentedCarRoutes = await carRouteService.AllRoutesByUserId(adminId);
            var agentId = await carDriverService.GetDriverId(adminId);
            myCarRoutes.AddedCarRoutes = await carRouteService.AllCarRoutesByDriverId(agentId);

            return View(myCarRoutes);
        }
    }
}
