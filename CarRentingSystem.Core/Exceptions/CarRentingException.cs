using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Exceptions
{
   
    public class CarRentingException : ApplicationException
    {
        public CarRentingException()
        {

        }

        public CarRentingException(string errorMessage)
            : base(errorMessage)
        {

        }
    }
}
