namespace CarRentingSystem.Core.Exceptions
{
    public class Guard : IGuard
    {
        public void AgainstNull<T>(T value, string? errorMessage = null)
        {
            if (value == null)
            {
                var exception = errorMessage == null ?
                    new CarRentingException() :
                    new CarRentingException(errorMessage);

                throw exception;
            }
        }
    }
}
