using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static CarRentingSystem.Infrastucture.Constants.ModelsConstants;


namespace CarRentingSystem.Infrastucture.Data
{
    [Comment("Shipment category")]
    public class Category
    {
        [Key]
        [Comment("Category Identifier")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(NameLength)]
        [Comment("Category name")]
        public string Name { get; set; } = string.Empty;

        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}

