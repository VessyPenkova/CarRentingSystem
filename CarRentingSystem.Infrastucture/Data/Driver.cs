using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CarRentingSystem.Infrastucture.Constants.ModelsConstants;


namespace CarRentingSystem.Infrastucture.Data
{
   
    [Comment("Driver route")]
    public class Driver
    {
        [Key]
        [Comment("Driver identifier")]
        public int DriverId { get; set; }

        [Required]
        [MaxLength(DriverPhoneMaxLength)]
        [Comment("Driver's phone")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Comment("User Identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
