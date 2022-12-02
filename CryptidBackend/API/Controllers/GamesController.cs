using Application.Games;
using Backend.Logic;
using Domain.Models;
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

        [HttpPost("{id}")]
        public async Task DeleteGame(Guid id)
        {
            HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult> GetGames()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }
    }
}
