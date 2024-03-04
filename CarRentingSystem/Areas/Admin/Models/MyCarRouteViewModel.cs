using CarRentingSystem.Core.Models.CarRoute;

namespace CarRentingSystem.Areas.Admin.Models
{

    public class MyCarRoutesViewModel
    {
        public IEnumerable<CarRouteServiceModel> AddedCarRoutes { get; set; }
            = new List<CarRouteServiceModel>();

        public IEnumerable<CarRouteServiceModel> RentedCarRoutes { get; set; }
            = new List<CarRouteServiceModel>();
    }

}
