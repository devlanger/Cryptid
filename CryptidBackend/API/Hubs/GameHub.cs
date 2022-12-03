using Cryptid.Shared;
using CryptidClient.Assets.Scripts.MapLoader;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Persistence.Data;
using static Cryptid.Backend.AuthService;

namespace Cryptid.Backend.Hubs
{
    public class GameHub : Hub<IGameClient>, IGameServer
    {
        private readonly ILogger<GameHub> logger;
        private readonly MatchmakingService matchmakingService;
        private readonly AuthService authService;
        private readonly ActionsController actionController;
        private readonly DataContext context;
        private readonly IMediator mediator;

        private GameState state;

        public GameHub(ILogger<GameHub> logger,
                       MatchmakingService matchmakingService,
                       AuthService authService,
                       IMediator mediator,
                       ActionsController controller,
                       DataContext context)
        {
            this.logger = logger;
            this.matchmakingService = matchmakingService;
            this.authService = authService;
            this.mediator = mediator;
            this.actionController = controller;
            this.context = context;
        }

        public override Task OnConnectedAsync()
        {
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
            // if (!Context.Items.ContainsKey("id"))
            // {
            //     return;
            // }

            logger.LogInformation($"{Context.ConnectionId} has joined the matchmaking queue");
            await Clients.Client(Context.ConnectionId).ChangeMenuState(1);
            matchmakingService.AddPlayerToMatchmaking(Context.ConnectionId);
        }

        public static Guid GLOBAL_GAME_ID = new Guid("809D6392-4D7A-48CB-A462-E001A7F640AD");

        public async Task ExecuteAction()
        {
            var game = await context.Games.FindAsync(GLOBAL_GAME_ID);
            if(game == null)
            {
                return;
            }
            
            var state = GameStateSerializationHelper.Load<GameState>(game.CurrentState);
            //actionController.Execute(state, null);
            logger.LogInformation($"{Context.ConnectionId} has executed action on game {GameHub.GLOBAL_GAME_ID}");
            string save = GameStateSerializationHelper.Save(game.CurrentState);
            game.CurrentState = save;
            await context.SaveChangesAsync();

            //await Clients.Client(Context.ConnectionId).ChangeMenuState(1);
            //matchmakingService.AddPlayerToMatchmaking(Context.ConnectionId);
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
                //var p = await usersRepository.CreateOrLoginPlayer(userId);
                //Context.Items["id"] = p.Id;
            }

            await Clients.Client(Context.ConnectionId).ChangeMenuState(0);
            matchmakingService.RemovePlayerMatchmaking(Context.ConnectionId);
        }
        #endregion
    }
}
