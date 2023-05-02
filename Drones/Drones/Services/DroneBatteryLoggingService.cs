using Drones.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Drones.Services
{
    public class DroneBatteryLogging
    {
        private readonly ILogger<DroneBatteryLoggingService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IDroneService _droneService;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(60);
        public DroneBatteryLogging(ILogger<DroneBatteryLoggingService> logger, IDroneService droneService)
        {
            _logger = logger;
            _droneService = droneService;
        }

        public async Task CheckDronesBattery()
        {
            var response = await _droneService.GetAllDrones();
            if (response.Data == null)
            {
                _logger.LogInformation("The drones fleet is empty.");
            }
            else
            {
                foreach (var drone in response.Data)
                {
                    _logger.LogInformation($"Drone {drone.SN} is at {drone.BatteryCapacity.ToString()} percent of battery.");
                }
            }
        }
    }
    public class DroneBatteryLoggingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(60);
        public DroneBatteryLoggingService( IServiceScopeFactory scopeFactory)
        {

            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_period, stoppingToken);
                await CheckDronesBattery();
            }
        }

        public async Task CheckDronesBattery()
        {
            await using AsyncServiceScope asyncServiceScope = _scopeFactory.CreateAsyncScope();
            DroneBatteryLogging _droneLogging = asyncServiceScope.ServiceProvider.GetRequiredService<DroneBatteryLogging>();
            await _droneLogging.CheckDronesBattery();
        }
    }


}
