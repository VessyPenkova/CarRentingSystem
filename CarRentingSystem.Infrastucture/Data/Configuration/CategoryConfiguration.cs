using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                    Name = "InterCitySingle"
                },
                new Category()
                {
                     CategoryId = 2,
                     Name = "InterCityShared"
                },
                new Category()
                {
                     CategoryId = 3,
                     Name = "OneWayLocal"
                },
                new Category()
                {
                     CategoryId = 4,
                     Name = "RoundTripLocal"
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
