using LinkO.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Persistence.Data.Configuration
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

        }
    }
}
