using Dropbox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropbox.Infrastructure.Persistence.Configurations
{
    public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.DeviceId).IsRequired();
            builder.Property(e => e.SyncFolder).IsRequired();
            builder.Property(e => e.RootFolderId);

            builder.HasOne(d => d.User)
             .WithMany(p => p.Devices)
             .HasForeignKey(d => d.UserId);

            builder.HasOne(d => d.Device)
             .WithMany(p => p.Users)
             .HasForeignKey(d => d.DeviceId);

            builder.HasOne(e => e.RootFolder)
                .WithOne(e => e.RootFolder)
                .HasForeignKey<Item>(e => e.RootFolderId);

            //builder.HasData(new UserDevice[] {
            //    new UserDevice
            //    {
            //        Id = new Guid("ee5b0fac-39d3-4206-8a5e-e1c7c9a13731"),
            //        UserId = new Guid("4caec599-49b1-4a09-96a7-5cf3556fa21e"),
            //        DeviceId = new Guid("7cc09ce3-9452-4653-be3f-e5d602feb187"),
            //        SyncFolder = "c:/Temp"
            //    },
            //});
        }
    }
}
