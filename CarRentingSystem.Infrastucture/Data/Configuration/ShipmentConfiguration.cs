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
               .HasOne(sh => sh.Category)
                .WithMany(c => c.Shipments)
                .HasForeignKey(sh => sh.CategId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(sh => sh.Driver)
                    .WithMany(d => d.Shipments)
                    .HasForeignKey(sh => sh.DriverId)
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

