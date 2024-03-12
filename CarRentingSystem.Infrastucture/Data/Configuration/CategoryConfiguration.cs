using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(CreateCategories());
        }
        private List<Category> CreateCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    CategoryId = 1,
                    Name = "InterCity Single"
                },
                new Category()
                {
                     CategoryId = 2,
                     Name = "InterCity Shared"
                },
                new Category()
                {
                     CategoryId = 3,
                     Name = "OneWay Local"
                },
                new Category()
                {
                     CategoryId = 4,
                     Name = "RoundTrip" +
                     "Local"
                },
                new Category()
                {
                     CategoryId = 5,
                     Name = "Luxury"
                },
                new Category()
                {
                     CategoryId = 6,
                     Name = "Charter"
                },
            };
            return categories;
        }
    }
}
