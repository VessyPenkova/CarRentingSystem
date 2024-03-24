using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Core.Models.Driver
{
    public class BecomeDriverModel
    {
        [Required]
        [StringLength(15, MinimumLength = 7)]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
