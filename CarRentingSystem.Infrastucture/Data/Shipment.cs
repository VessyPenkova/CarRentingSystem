using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CarRentingSystem.Infrastucture.Constants.ModelsConstants;

namespace CarRentingSystem.Infrastucture.Data
{
    [Comment("Shipment to book")]
    public class Shipment
    {
        [Key]
        [Comment("Shipment Identifier")]
        public int ShipmentId { get; set; }

        [Required, MaxLength(ShipmentTitleMaxLength)]
        [Comment("Title")]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(ShipmentLoadingAddressMaxLength)]
        [Comment("Loading Address")]
        public string LoadingAddress { get; set; } = string.Empty;

        [Required, MaxLength(ShipmentDeliveryAddressMaxLength)]
        [Comment("Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required, MaxLength(ShipmentDescriptionMaxLength)]
        [Comment("Shipment description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("Shipment image url")]
        public string ImageUrlShipmentGoogleMaps { get; set; } = string.Empty;

        [Required, Comment("Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required, Comment("Category identifier")]
        public int CategId { get; set; }
        [ForeignKey(nameof(CategId))]
        public Category Category { get; set; } = null!;

        // Driver is OPTIONAL at create time
        [Comment("Driver identifier")]
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }

        // Renter is OPTIONAL
        [Comment("User id of the rentier")]
        public string? RenterId { get; set; }
        public User? Renter { get; set; }

        // Creator is REQUIRED (the user who posted the shipment)
        [Required]
        [Comment("User id of the creator")]
        public string CreatorId { get; set; } = null!;
        [ForeignKey(nameof(CreatorId))]
        public User Creator { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
