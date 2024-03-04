using System.ComponentModel.DataAnnotations;


namespace CarRentingSystem.Infrastucture.Data
{
    public class Category
    {
        public Category()
        {
            CarRoutes = new List<CarRoute>();
        }

        [Key]
        public int CategoryId { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;


        public List<CarRoute> CarRoutes  { get; set; }
    }
}

