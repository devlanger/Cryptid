using Backend.Logic;
using Backend.Repositories;
using Cryptid.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static Cryptid.Backend.AuthService;

namespace Cryptid.Backend.Hubs
{
    public class GameHub : Hub<IGameClient>, IGameServer
    {
        private readonly ILogger<GameHub> logger;
        private readonly IGamesController gamesController;
        private readonly MatchmakingService matchmakingService;
        private readonly AuthService authService;
        private readonly IUsersRepository usersRepository;

        public GameHub(
            ILogger<GameHub> logger,
            IGamesController gamesController, 
            MatchmakingService matchmakingService,
            AuthService authService,
            IUsersRepository usersRepository)
        {
            this.logger = logger;
            this.gamesController = gamesController;
            this.matchmakingService = matchmakingService;
            this.authService = authService;
            this.usersRepository = usersRepository;
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
        public async Task AskToJoinMatchmaking()
        {
            //TODO: Check if logged
            if (!Context.Items.ContainsKey("id"))
            {
                return;
            }

            logger.LogInformation($"{Context.ConnectionId} has joined the matchmaking queue");
            await Clients.Client(Context.ConnectionId).ChangeMenuState(1);
            matchmakingService.AddPlayerToMatchmaking(Context.ConnectionId);
        }

        public async Task AskToRemoveMatchmaking()
        {
            logger.LogInformation($"{Context.ConnectionId} has left the matchmaking queue");
            await Clients.Client(Context.ConnectionId).ChangeMenuState(0);
            matchmakingService.RemovePlayerMatchmaking(Context.ConnectionId);
        }

        public async Task SetNickname(string nickname)
        {
            await Clients.Client(Context.ConnectionId).ChangeMenuState(0);
        }

        public async Task LoginWithAccessToken(string userId, string accessToken)
        {
            logger.LogInformation($"{Context.ConnectionId} has login with user id {userId}");
            UnityAuthModel result = authService.Authenticate(userId, accessToken);
            logger.LogInformation($"Login state: {result.id} {result.disabled}");

            //TODO: CHECK IF - DISABLED IS LOGGED STATE?

            if(result.id == "")
            {
                logger.LogInformation("Player isn't logged");
            }
            else
            {
                //gamesController.CreateGame(GameType.TEAM);
                var p = await usersRepository.CreateOrLoginPlayer(userId);
                Context.Items["id"] = p.Id;
            }

            await Clients.Client(Context.ConnectionId).ChangeMenuState(0);
            matchmakingService.RemovePlayerMatchmaking(Context.ConnectionId);
        }
        #endregion
    }
}
