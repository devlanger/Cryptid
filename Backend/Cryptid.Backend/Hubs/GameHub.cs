using Backend.Logic;
using Microsoft.AspNetCore.SignalR;

namespace Cryptid.Backend.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGamesController gamesController;
        private readonly MatchmakingService matchmakingService;

        public GameHub(
            IGamesController gamesController, MatchmakingService matchmakingService)
        {
            this.gamesController = gamesController;
            this.matchmakingService = matchmakingService;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected: {Context.ConnectionId} {gamesController.GamesAmount()}");
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", "127.0.0.1:1999");

            matchmakingService.AddPlayerToMatchmaking(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            matchmakingService.RemovePlayerMatchmaking(Context.ConnectionId);
            Console.WriteLine($"Disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SearchForGame()
        {
            await Clients.All.SendAsync("ReceiveGame", "127.0.0.1:1999");
        }
    }
}
