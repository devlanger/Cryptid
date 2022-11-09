using Cryptid.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Logic
{
    public class CryptidBackendWorker : BackgroundService
    {
        private readonly IGamesController gamesController;
        private readonly MatchmakingService matchmakingService;
        private readonly ILogger<CryptidBackendWorker> logger;

        public CryptidBackendWorker(
            ILogger<CryptidBackendWorker> logger, 
            IGamesController gamesController,
            MatchmakingService matchmakingService)
        {
            this.gamesController = gamesController;
            this.logger = logger;
            this.matchmakingService = matchmakingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Started server version hash {VersionManager.GitHash}");

            while (!stoppingToken.IsCancellationRequested)
            {
                await matchmakingService.MatchPlayers();
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
