using CarRentingSystem.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Core.Models.Shipment
{
    public class ShipmentModel: IShipmentModel
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
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrlShipmentGoogleMaps { get; set; } = null!;

        [Required]
        [Display(Name = "Price")]
        [Range(0.00, 2000.00, ErrorMessage = "Price must be a positive number and more than {10} leva")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public int CategId { get; set; }

        public IEnumerable<ShipmentCategoryModel> ShipmentCategories { get; set; } = new List<ShipmentCategoryModel>();
    }
}
