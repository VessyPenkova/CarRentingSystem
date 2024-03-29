using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Models.Drivers
{
    public class BecomeDriverFormModel
    {
        [Required]
        [StringLength(15, MinimumLength = 7)]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
