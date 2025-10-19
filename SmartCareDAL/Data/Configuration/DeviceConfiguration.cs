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
            builder.ToTable("Devices");
            builder.HasKey(x => x.Id); 

            builder.Property(x => x.DeviceIdentifier).HasMaxLength(100).IsRequired(); 

            builder.Property(x => x.Model).HasMaxLength(100); 

            builder.Property(x => x.LastSeenDate).HasColumnType("date"); 

            builder.Property(x => x.LastSeenTime); 

            builder.Property(x => x.IsActive).IsRequired(); 

            builder.Property(x => x.IsPaired).IsRequired();

            builder.HasOne(x => x.User).WithMany(u => u.Devices).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
