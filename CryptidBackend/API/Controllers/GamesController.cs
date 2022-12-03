using Application.Games;
using Backend.Logic;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public interface IGamesController
    {
        Task DeleteGame(Guid gameId);
        Task<Game> CreateGame(GameType type);
        Task<Game> GetGame(Guid gameId);
        int GamesAmount();
        Task AddGameParticipants(Guid id, string playerId, string player);
    }

    [Authorize]
    public class GamesController : BaseApiController
    {
        private readonly IGameFactory gamesFactory;
        private readonly ILogger<GamesController> logger;

        public GamesController(
            ILogger<GamesController> logger,
            IGameFactory gamesFactory)
        {
            this.gamesFactory = gamesFactory;
            this.logger = logger;
        }

        [HttpDelete("{id}")]
        public async Task DeleteGame(Guid id)
        {
            HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
        

        [HttpGet("user/{id}")]
        public async Task<ActionResult> GetGamesForUser(string id)
        {
            return HandleResult(await Mediator.Send(new ListForUser.Query() { UserId = id }));
        }

        [HttpGet]
        public async Task<ActionResult> GetGames()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
    }
}
