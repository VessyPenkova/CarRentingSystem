using CarRentingSystem.Core.Models.DriverCar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.CarRoute
{
    public  class CarRouteDetailsModel: CarRouteServiceModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public DriverCarServiceModel DriverCar { get; set; } = null!;
    }
}
