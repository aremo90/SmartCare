using LinkO.Domin.Contract;
using LinkO.Persistence.IdentityData.DbContext;
using Microsoft.EntityFrameworkCore;

namespace SmartCareAPI.Extensions
{
    public static class WebAppResisteration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbContextService = Scope.ServiceProvider.GetRequiredService<LinkOIdentityDbContext>();

            var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();


            if (pendingMigrations.Any())
                await dbContextService.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var DataInitilizerService =  Scope.ServiceProvider.GetRequiredKeyedService<IDataInitilizer>("Default");
            await DataInitilizerService.InitilizeAsync();
            return app;
        }
        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var DataInitilizerService =  Scope.ServiceProvider.GetRequiredKeyedService<IDataInitilizer>("Identity");
            await DataInitilizerService.InitilizeAsync();
            return app;
        }
    }
}
