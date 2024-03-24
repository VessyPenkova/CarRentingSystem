using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            var data = new ConfigureData();

            builder.HasData(new Category[] 
            {
             data.LuxuryCategory,
             data.CharterCategory, 
             data.InterCityCategory, 
             data.OneWayCategory,
             data.RoundShipmentCategory
            });
        }
    }
}
