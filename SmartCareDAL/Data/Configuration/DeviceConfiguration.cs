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

            builder.Property(d => d.Model)
                   .HasMaxLength(100);

            builder.Property(d => d.IsActive)
                   .IsRequired();

            builder.Property(d => d.IsPaired)
                   .HasDefaultValue(false);

            builder.Property(d => d.SignalStrength)
                   .HasColumnType("float");

            builder.HasOne(d => d.User)
                .WithOne(u => u.Device)
                .HasForeignKey<Device>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
