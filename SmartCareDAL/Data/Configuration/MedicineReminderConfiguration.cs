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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MedicationName)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(x => x.Dosage)
                     .IsRequired();
            builder.Property(x => x.Unit)
                     .IsRequired()
                     .HasMaxLength(50);
            builder.Property(x => x.MedicationType)
                     .IsRequired()
                     .HasMaxLength(100);
            builder.Property(x => x.Frequency)
                     .IsRequired();
            builder.Property(x => x.StartDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(x => x.ScheduleTime)
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(u => u.MedicineReminders)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
