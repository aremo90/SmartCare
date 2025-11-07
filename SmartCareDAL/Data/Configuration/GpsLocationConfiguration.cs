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
    internal class GpsLocationConfiguration : IEntityTypeConfiguration<GpsLocation>
    {
        public void Configure(EntityTypeBuilder<GpsLocation> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Latitude)
                .IsRequired();

            builder.Property(g => g.Longitude)
                .IsRequired();

            builder.HasIndex(g => g.UserId)
                   .IsUnique();
        }
    }
}
