using Backend.Logic;
using Cryptid.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Cryptid.Backend.Hubs
{
    public class GameHub : Hub<IGameClient>, IGameServer
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
            Clients.Client(Context.ConnectionId).SendMessage("ReceiveGame", "127.0.0.1:1999");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            matchmakingService.RemovePlayerMatchmaking(Context.ConnectionId);
            Console.WriteLine($"Disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        #region Commands 
        public async void AskToJoinMatchmaking()
        {
            await Clients.Client(Context.ConnectionId).ChangeMenuState(1);
            matchmakingService.AddPlayerToMatchmaking(Context.ConnectionId);
        }

        public async void AskToRemoveMatchmaking()
        {
            await Clients.Client(Context.ConnectionId).ChangeMenuState(0);
            matchmakingService.RemovePlayerMatchmaking(Context.ConnectionId);
        }
        #endregion
    }
}
