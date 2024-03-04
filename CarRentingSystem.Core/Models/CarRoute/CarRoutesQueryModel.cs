using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.CarRoute
{
    public class CarRoutesQueryModel: CarRouteModel
    {
        public int TotalCarRouteCount { get; set; }

        public IEnumerable<CarRouteServiceModel> CarRoutes { get; set; } = new List<CarRouteServiceModel>();
    }
}

