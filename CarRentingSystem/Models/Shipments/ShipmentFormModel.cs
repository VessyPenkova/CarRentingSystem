using CarRentingSystem.Core.Models.Shipment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CarRentingSystem.Core.Contracts.Shipments;
using CarRentingSystem.Infrastucture.Data;
using static CarRentingSystem.Infrastucture.Constants.ModelsConstants;

namespace CarRentingSystem.Models.Shipments
{
    public class ShipmentFormModel : IShipmentModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string LoadingAddress { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string DeliveryAddress { get; set; } = null!;

        [Required]
        [StringLength(ShipmentDescriptionMaxLength, MinimumLength = ShipmentDescriptionMinLength)]
        public string Description { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrlShipmentGoogleMaps { get; set; } = null!;

        [Required]
        [Display(Name = "Price")]
        [Range(0.00, 2000.00, ErrorMessage = "Price must be a positive number and more than {10} leva")]
        public decimal Price { get; set; }

        [DisplayName("Is Rented")]
        public bool IsRented { get; init; }

        public Category Category { get; set; } = null!;

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<ShipmentCategoryServiceModel> ShipmentCategories { get; set; } = new List<ShipmentCategoryServiceModel>();
    }
}
