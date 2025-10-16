using Microsoft.EntityFrameworkCore;
using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Data.Context
{
    public class SmartCareDbContext : DbContext
    {
        public SmartCareDbContext(DbContextOptions<SmartCareDbContext> options)
                    : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations from Configurations folder
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MedicineReminder> MedicineReminders { get; set; }
        public DbSet<GpsLocation> GpsLocations { get; set; }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
