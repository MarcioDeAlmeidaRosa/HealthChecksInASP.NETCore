﻿using Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.HealthCheck;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.Services
{
    #region snippet1
    public class StartupHostedService : IHostedService, IDisposable
    {
        private readonly int _delaySeconds = 10;
        private readonly ILogger _logger;
        private readonly StartupHostedServiceHealthCheck _startupHostedServiceHealthCheck;

        public StartupHostedService(ILogger<StartupHostedService> logger,
            StartupHostedServiceHealthCheck startupHostedServiceHealthCheck)
        {
            _logger = logger;
            _startupHostedServiceHealthCheck = startupHostedServiceHealthCheck;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Startup Background Service is starting.");

            Task.Run(async () =>
            {
                await Task.Delay(_delaySeconds * 1000);

                _startupHostedServiceHealthCheck.StartupTaskCompleted = true;

                _logger.LogInformation($"Startup Background Service has started.");
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Startup Background Service is stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
    #endregion
}
