using LinkO.ServiceAbstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCareAPI.Hosted
{
    public class MedicineNotificationHostedService : BackgroundService
    {
        private readonly ILogger<MedicineNotificationHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public MedicineNotificationHostedService(ILogger<MedicineNotificationHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Medicine notification hosted service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // --- 1. Do the work ---
                    using var scope = _scopeFactory.CreateScope();
                    var medicineService = scope.ServiceProvider.GetRequiredService<IMedicineService>();

                    _logger.LogDebug("Running SendNotificationAsync at {time}", DateTimeOffset.Now);

                    var result = await medicineService.SendNotificationAsync();

                    if (result.IsSuccess)
                        _logger.LogInformation("Success: {msg}", result.Value);
                    else
                        _logger.LogWarning("Failed: {err}", System.Text.Json.JsonSerializer.Serialize(result.Errors));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while running medicine notification scan.");
                }

                try
                {
                    // --- 2. Calculate delay to snap to the next minute ---
                    // This ensures that if the work finished at 8:00:50, we only wait 10 seconds 
                    // so the next run happens exactly at 8:01:00.

                    var now = DateTimeOffset.Now;
                    var secondsUntilNextMinute = 60 - now.Second;
                    var millisecondsDelay = (secondsUntilNextMinute * 1000) - now.Millisecond;

                    // Safety check: ensure we don't try to wait a negative time 
                    if (millisecondsDelay <= 0) millisecondsDelay += 60000;

                    await Task.Delay(millisecondsDelay, stoppingToken);
                }
                catch (TaskCanceledException) { /* shutting down */ }
            }
        }
    }
}