using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Data.Configuration
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.DeviceIdentifier)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.DeviceName)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.HasIndex(d => d.UserId)
                   .IsUnique();
        }
    }
}
