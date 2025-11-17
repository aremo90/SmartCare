using Microsoft.EntityFrameworkCore;
using SmartCareDAL.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCareDAL.Data.Context
{
    public partial class SmartCareDbContext
    {
        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added ||
                        e.State == EntityState.Modified));

            if (!entries.Any())
            {
                return;
            }

            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var cairoTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone);

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = cairoTime;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = cairoTime;
                }
            }
        }
    }
}