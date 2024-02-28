using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Infrastucture.Data
{
    public class ApplicationUser :  IdentityUser<string>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool? IsActive { get; set; } = true;

        //public int  UserId { get; set; }
    }
}
