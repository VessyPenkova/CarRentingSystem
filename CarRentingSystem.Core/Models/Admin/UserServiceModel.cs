using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.Admin
{
    public class UserServiceModel
    {
        public string UserId { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string FullName { get; init; } = null!;

        public string? PhoneNumber { get; init; } = null;
    }
}
