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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Gender)
                .IsRequired();

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11)
                .HasAnnotation("RegularExpression", @"^01[0-9]{9}$");

            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.MedicineReminders)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(u => u.Device)
                .WithOne(d => d.User)
                .HasForeignKey<Device>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.GpsLocation)
                .WithOne(g => g.User)
                .HasForeignKey<GpsLocation>(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
