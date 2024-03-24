using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingSystem.Infrastucture.Data.Configuration
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder
               .HasOne(h => h.Category)
                .WithMany(c => c.Shipments)
                .HasForeignKey(h => h.CategId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.Driver)
                    .WithMany(a => a.Shipments)
                    .HasForeignKey(h => h.DriverId)
                    .OnDelete(DeleteBehavior.Restrict);

            var data = new ConfigureData();

            builder.HasData(new Shipment[] 
            { 
                data.CharterShipment,
                data.LuxuryShipment,
                data.OneWayShipment,
                data.RoundShipment
            });
        }
    }
}

