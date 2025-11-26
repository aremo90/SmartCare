using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartCareBLL.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Hosted
{
    public class ReminderUpdateHostedService : BackgroundService
    {
        private readonly ILogger<ReminderUpdateHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ReminderUpdateHostedService(ILogger<ReminderUpdateHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reminder Update Hosted Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var medicineService = scope.ServiceProvider.GetRequiredService<IMedicineService>();
                        _logger.LogInformation("Checking for past-due reminders to update at {time}", DateTimeOffset.Now);
                        
                        await medicineService.ProcessPastDueRemindersAsync();

                        _logger.LogInformation("Finished checking for past-due reminders.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating reminders.");
                }

                // Wait before running again
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
