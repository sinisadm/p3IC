using Dropbox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropbox.Infrastructure.Persistence.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.Property(e => e.MacAddress)
                .IsRequired()
                .HasMaxLength(17);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            //builder.HasData(new Device[] {
            //        new Device
            //        {
            //            Id = new Guid("7cc09ce3-9452-4653-be3f-e5d602feb187"),
            //            MacAddress = "00-14-22-01-23-45",
            //            Name = "Kompjuter 1"
            //        },
            //});
        }
    }
}
