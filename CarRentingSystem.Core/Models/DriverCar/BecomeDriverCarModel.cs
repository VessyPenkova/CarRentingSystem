using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.DriverCar
{
    public class BecomeDriverCarModel
    {
        [Required]
        [StringLength(15, MinimumLength = 7)]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
