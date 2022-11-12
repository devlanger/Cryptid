using Backend.Data;
using Backend.Repositories;
using Cryptid.Backend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Logic
{
    public interface IGamesController
    {
        Task RemoveGame(Guid gameId);
        Task<Game> CreateGame(GameType type);
        Task<Game> GetGame(Guid gameId);
        int GamesAmount();
        Task AddGameParticipants(Guid id, string playerId, string player);
    }

    public class GamesController : IGamesController 
    {
        private readonly IGameFactory gamesFactory;
        private readonly ILogger<GamesController> logger;
        private readonly GamesRepository gamesRepository;

        public GamesController(
            ILogger<GamesController> logger,
            IGameFactory gamesFactory,
            GamesRepository gamesRepository)
        {
            this.gamesFactory = gamesFactory;
            this.logger = logger;
            this.gamesRepository = gamesRepository;
        }

        public async Task RemoveGame(Guid gameId)
        {
            await gamesRepository.DeleteGame(gameId);
        }

        public async Task AddGameParticipants(Guid id, string playerId, string player)
        {
            await gamesRepository.AddGameParticipants(id, playerId, player);
        }

        public async Task<Game> CreateGame(GameType type)
        {
            var game = (Game)gamesFactory.CreateGame(type);
            game.CurrentState = new GameState().GetCompressedState();
            await gamesRepository.SaveGame(game);
            logger.LogInformation($"Create game id: {game.Id} type: {type}");
            return game;
        }

        public async Task<Game> GetGame(Guid gameId)
        {
            return await gamesRepository.GetGame(gameId);
        }

        public async Task<List<Game>> GetAllGamesForPlayer(Guid playerId)
        {
            return await gamesRepository.GetAllGamesForPlayer(playerId);
        }

        public int GamesAmount()
        {
            return 0;
        }
    }
}
