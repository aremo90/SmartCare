using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Persistence.IdentityData.DbContext
{
    public class LinkOIdentityDbContext : IdentityDbContext<ApplicationUser>
    {

        public LinkOIdentityDbContext(DbContextOptions<LinkOIdentityDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationUser>().
                Property(x => x.UserName)
                .IsRequired();


            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<Device>().ToTable("Devices");
            builder.Entity<GpsLocation>().ToTable("GpsLocations");
            builder.Entity<MedicineReminder>()
                .ToTable("MedicineReminders")
                .HasOne(r => r.User)
                .WithMany(u => u.MedicineReminders)
                .HasForeignKey(r => r.UserId);

        }

    }
}
