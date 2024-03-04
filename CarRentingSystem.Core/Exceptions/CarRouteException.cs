using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Exceptions
{
    public class CarRouteException: ApplicationException
    {
        public CarRouteException()
        {

        }

        public CarRouteException(string errorMessage)
            : base(errorMessage)
        {

        }
    }
}
