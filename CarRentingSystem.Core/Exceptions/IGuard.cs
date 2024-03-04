using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Exceptions
{
    public interface IGuard
    {
        void AgainstNull<T>(T value, string? errorMessage = null);
    }
}
