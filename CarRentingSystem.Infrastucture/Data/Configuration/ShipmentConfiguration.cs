using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasOne(s => s.Category)
                   .WithMany(c => c.Shipments)
                   .HasForeignKey(s => s.CategId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Driver)
                   .WithMany(d => d.Shipments)
                   .HasForeignKey(s => s.DriverId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Creator)
                   .WithMany()
                   .HasForeignKey(s => s.CreatorId)
                   .OnDelete(DeleteBehavior.Restrict);

            var data = new ConfigureData();
            builder.HasData(new Shipment[]
            {
                data.OneWayShipment,
                data.RoundShipment,
                data.LuxuryShipment,
                data.CharterShipment
            });
        }
    }
}
