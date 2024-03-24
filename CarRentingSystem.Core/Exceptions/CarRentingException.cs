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
