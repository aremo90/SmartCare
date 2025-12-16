using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.OrderModule;
using LinkO.Persistence.IdentityData.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkO.Persistence.DataSeed
{
    public class DataInitilizer : IDataInitilizer
    {
        private readonly LinkOIdentityDbContext _dbContext;
        private readonly ILogger<DataInitilizer> _logger;

        public DataInitilizer(LinkOIdentityDbContext dbContext , ILogger<DataInitilizer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task InitilizeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Set<Product>().AnyAsync();
                var HasTypes = await _dbContext.Set<ProductType>().AnyAsync();
                var HasDelivery = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasProducts && HasTypes && HasDelivery) return;
                
                if (!HasTypes)
                    await SeedDataFromJsonAsync<ProductType , int>("ProductTypes.json" , _dbContext.Set<ProductType>());
                _dbContext.SaveChanges();

                if (!HasProducts)
                    await  SeedDataFromJsonAsync<Product , int>("Product.json", _dbContext.Set<Product>());

                if (!HasDelivery)
                    await SeedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while seeding data from JSON file: {ex}");
            }
        }

        private async Task SeedDataFromJsonAsync<T , Tkey>(string fileName, DbSet<T> dbSet) where T : BaseEntity<Tkey>
        {
            var FilePath = @"..\LinkO.Persistence\DataSeed\JSON\" + fileName;

            if (!File.Exists(FilePath))
                throw new FileNotFoundException("The specified JSON file was not found.", FilePath);

            try
            {
                using var DataStream = File.OpenRead(FilePath);

                var Data = JsonSerializer.Deserialize<List<T>>(DataStream , new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                if (Data is not null)
                {
                    await dbSet.AddRangeAsync(Data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding data from JSON file: {FileName}", fileName);
            }

        }
    }
}
