using Dropbox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropbox.Infrastructure.Persistence.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(e => e.ParentItemId);
            builder.Property(e => e.IsFolder).IsRequired();
            builder.Property(e => e.UserDeviceId).IsRequired();
            //builder.Property(e => e.Version).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.ItemName).IsRequired();
            //builder.Property(e => e.ItemExtension).IsRequired();
            //builder.Property(e => e.ItemSize).IsRequired();
            //builder.Property(e => e.ItemPath).IsRequired();

            builder.HasOne(e => e.UserDevice)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.UserDeviceId);

            builder.HasOne(d => d.ParentItem)
             .WithMany(p => p.Members)
             .HasForeignKey(d => d.ParentItemId)
             .OnDelete(DeleteBehavior.Cascade);
            //.OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
