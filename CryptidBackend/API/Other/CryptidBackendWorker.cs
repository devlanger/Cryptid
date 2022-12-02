using Cryptid.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Logic
{
    public class CryptidBackendService : BackgroundService
    {
        private readonly ILogger<CryptidBackendService> logger;
        private readonly IServiceProvider services;

        public CryptidBackendService(
            ILogger<CryptidBackendService> logger, 
            IServiceProvider services)
        {
            this.logger = logger;
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Started server version hash {VersionManager.GitHash}");

            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
                await Task.Delay(3000, stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = services.CreateScope())
            {
                var matchmakingService = scope.ServiceProvider.GetRequiredService<MatchmakingService>();
                await matchmakingService.MatchPlayers();
            }
        }
    }
}
