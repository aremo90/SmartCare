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

            builder.Property(x => x.MedicineName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Dosage)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.ScheduleDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(x => x.ScheduleTime)
                   .IsRequired();

            builder.Property(x => x.RepeatPattern)
                   .HasConversion<int>() // store enum as int
                   .IsRequired();

            builder.Property(x => x.DaysOfWeek)
                   .HasMaxLength(50);

            builder.HasOne(x => x.User)
                   .WithMany(u => u.MedicineReminders)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
