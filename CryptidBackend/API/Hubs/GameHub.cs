using Cryptid.Shared;
using CryptidClient.Assets.Scripts.MapLoader;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Persistence.Data;
using System.Security.Claims;
using static Cryptid.Backend.AuthService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cryptid.Backend.Hubs
{
    //[Authorize]
    public class GameHub : Hub<IGameClient>, IGameServer
    {
        private readonly ILogger<GameHub> logger;
        private readonly MatchmakingService matchmakingService;
        private readonly AuthService authService;
        private readonly ActionsController actionController;
        private readonly DataContext context;
        private readonly IMediator mediator;

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
            await Clients.Client(Context.ConnectionId).ChangeMenuState(1);
            matchmakingService.AddPlayerToMatchmaking(GetUserId());
        }

        public async Task AskToRemoveMatchmaking()
        {
            await Clients.Client(Context.ConnectionId).ChangeMenuState(0);
            matchmakingService.RemovePlayerMatchmaking(GetUserId());
        }

        public async Task SendActionCommand(byte[] bytes)
        {
            CommandBase command = CommandReader.ReadCommandFromBytes(bytes);

            var game = await context.Games.FindAsync(new Guid(command.gameId));
            if (game == null)
            {
                return;
            }

            var state = GameStateSerializationHelper.Load<GameState>(game.CurrentState);

            if (command == null)
            {
                logger.LogError($"Wrong command execution {GetUserId()}");
            }
            else
            {
                var action = ActionFactory.CreateActionFromCommand(state, command);
                command.PlayerId = GetUserId();

                var result = actionController.Execute(state, action, command);
                if (result.IsSuccess)
                {
                    await Clients.Users(state.players.Keys.ToList()).HandleActionCommand(bytes);
                    logger.LogInformation($"{Context.ConnectionId} has executed action on game");
                    string save = GameStateSerializationHelper.Save(state);
                    game.CurrentState = save;
                    await context.SaveChangesAsync();
                }
                else
                {
                    logger.LogError(result.Error);
                }
            }
        }

        private string GetUserId()
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            string id = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id;
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
