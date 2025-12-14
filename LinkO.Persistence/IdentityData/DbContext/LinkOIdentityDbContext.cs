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

            builder.Entity<Address>().ToTable("Addresses")
                .HasOne(r => r.User)
                .WithMany(u => u.Address)
                .HasForeignKey(r => r.UserId); ;
            builder.Entity<Device>().ToTable("Devices");
            builder.Entity<GpsLocation>().ToTable("GpsLocations");
            builder.Entity<MedicineReminder>()
                .ToTable("MedicineReminders")
                .HasOne(r => r.User)
                .WithMany(u => u.MedicineReminders)
                .HasForeignKey(r => r.UserId);

            builder.Entity<Product>().Property(X => X.Name).HasMaxLength(100);
            builder.Entity<Product>().Property(X => X.Description).HasMaxLength(500);
            builder.Entity<Product>().Property(X => X.ImageUrl).HasMaxLength(200);
            builder.Entity<Product>().Property(X => X.ImageAlt).HasMaxLength(200);

            builder.Entity<Product>()
                .HasOne(pt => pt.ProductType)
                .WithMany()
                .HasForeignKey(pt => pt.TypeId);

        }
        DbSet<Product> Products { get; set; }
        DbSet<ProductType> ProductTypes { get; set; }
        }
}
