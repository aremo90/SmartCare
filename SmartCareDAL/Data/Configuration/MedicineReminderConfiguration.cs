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
    internal class MedicineReminderConfiguration : IEntityTypeConfiguration<MedicineReminder>
    {
        public void Configure(EntityTypeBuilder<MedicineReminder> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MedicineName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Dosage)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.ReminderTime)
                .IsRequired();
        }
    }
}
