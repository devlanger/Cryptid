using Backend.Logic;
using Cryptid.Backend.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Cryptid.Backend
{
    public class MatchmakingService
    {
        public HashSet<string> waitingPlayers = new HashSet<string>();

        private readonly IHubContext<GameHub> hub;
        private readonly IGamesController gamesController;
        private readonly ILogger<MatchmakingService> logger;

        public MatchmakingService(
            ILogger<MatchmakingService> logger,
            IHubContext<GameHub> hub,
            IGamesController gamesController)
        {
            this.logger = logger;
            this.hub = hub;
            this.gamesController = gamesController;
        }

        public void AddPlayerToMatchmaking(string id)
        {
            waitingPlayers.Add(id);
        }

        public void RemovePlayerMatchmaking(string id)
        {
            waitingPlayers.Remove(id);
        }

        public async Task MatchPlayers()
        {
            logger.LogInformation($"Matchmaking update {DateTimeOffset.Now} Players waiting: {waitingPlayers.Count}");

            string lastPlayer = "";
            foreach (var player in waitingPlayers.ToList())
            {
                if(string.IsNullOrEmpty(lastPlayer))
                {
                    lastPlayer = player;
                }
                else
                {
                    await MatchSelectedPlayers(lastPlayer, player);
                    waitingPlayers.Remove(lastPlayer);
                    waitingPlayers.Remove(player);
                }
            }
        }

        private async Task MatchSelectedPlayers(string playerId, string player)
        {
            logger.LogInformation($"Log player {playerId} with player {player}");

            var game = gamesController.CreateGame(GameType.TEAM);
            //game.AddPlayers();
            
            await hub.Clients.Client(playerId).SendAsync("found_player", player);
            await hub.Clients.Client(player).SendAsync("found_player", playerId);
        }
    }
}
