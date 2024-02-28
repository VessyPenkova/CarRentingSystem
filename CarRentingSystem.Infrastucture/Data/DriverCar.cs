using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Infrastucture.Data
{
    public class DriverCar
    {
        
            [Key]
            public int DriverCarId { get; set; }

            [Required]
            [StringLength(15)]
            public string PhoneNumber { get; set; } = null!;

            [Required]
            public string UserId { get; set; } = null!;

            [ForeignKey(nameof(UserId))]
            public ApplicationUser User { get; set; } = null!;
            //public IdentityUser User { get; set; } = null!;

    }
}
