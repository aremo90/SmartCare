using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;



namespace SmartCareDAL.Data.Context
{
    public class SmartCareDbContextFactory : IDesignTimeDbContextFactory<SmartCareDbContext>
    {
        public SmartCareDbContext CreateDbContext(string[] args)
        {
            // Load configuration from SmartCareAPI (the startup project)
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
                .Build();


            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SmartCareDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SmartCareDbContext(optionsBuilder.Options);
        }
    }
}
