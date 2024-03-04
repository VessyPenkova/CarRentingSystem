using CarRentingSystem.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Core.Models.CarRoute
{
    public class CarRouteModel: ICarRouteModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string PickUpAddress { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string DeliveryAddress { get; set; } = null!;

        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrlRouteGoogleMaps { get; set; } = null!;

        [Required]
        [Display(Name = "Price per month")]
        [Range(0.00, 2000.00, ErrorMessage = "Price must be a positive number and less than {2} leva")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CarRouteCategoryModel> CarRouteCategories { get; set; } = new List<CarRouteCategoryModel>();
    }
}
