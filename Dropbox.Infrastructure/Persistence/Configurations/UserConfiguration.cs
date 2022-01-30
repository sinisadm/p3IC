using Dropbox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropbox.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.QuotaLimit);
            builder.Property(e => e.OuotaUsed);


            //builder.HasData(new User[] {
            //        new User
            //        {
            //            Id = new Guid("4caec599-49b1-4a09-96a7-5cf3556fa21e"),
            //            Email = "sinisadm@gmail.com",
            //            QuotaLimit = 2147483648,
            //            OuotaUsed = 0
            //        },
            //});
        }
    }
}
