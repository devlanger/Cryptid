using Backend.Data;
using Backend.Logic;
using Cryptid.Backend.Hubs;
using Cryptid.Backend.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Cryptid.Backend
{
    public class MatchmakingService
    {
        private readonly IHubContext<GameHub> hub;
        private readonly IGamesController gamesController;
        private readonly ILogger<MatchmakingService> logger;
        private readonly CacheRepository cacheRepository;

        public MatchmakingService(
            ILogger<MatchmakingService> logger,
            IHubContext<GameHub> hub,
            IGamesController gamesController,
            CacheRepository cacheRepository)
        {
            this.logger = logger;
            this.hub = hub;
            this.gamesController = gamesController;
            this.cacheRepository = cacheRepository;
        }

        public void AddPlayerToMatchmaking(string id)
        {
            cacheRepository.AddPlayerToMatchmaking(id);
        }

        public void RemovePlayerMatchmaking(string id)
        {
            cacheRepository.RemovePlayerMatchmaking(id);
        }

        public async Task MatchPlayers()
        {
            logger.LogInformation($"Matchmaking update {DateTimeOffset.Now} Players waiting: {cacheRepository.WaitingPlayers.Count}");

            string lastPlayer = "";
            foreach (var player in cacheRepository.WaitingPlayers.ToList())
            {
                if(string.IsNullOrEmpty(lastPlayer))
                {
                    lastPlayer = player;
                }
                else
                {
                    await MatchSelectedPlayers(lastPlayer, player);
                    cacheRepository.RemovePlayerMatchmaking(lastPlayer);
                    cacheRepository.RemovePlayerMatchmaking(player);
                }
            }
        }

        private async Task MatchSelectedPlayers(string playerId, string player)
        {
            logger.LogInformation($"Log player {playerId} with player {player}");

            var game = await gamesController.CreateGame(GameType.TEAM);
            await gamesController.AddGameParticipants(game.Id, playerId, player);

            await hub.Clients.Client(playerId).SendAsync("ChangeMenuState", 2);
            await hub.Clients.Client(player).SendAsync("ChangeMenuState", 2);
        }
    }
}
