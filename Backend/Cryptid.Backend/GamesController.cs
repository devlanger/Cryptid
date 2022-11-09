using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Logic
{
    public interface IGamesController
    {
        void RemoveGame(Guid gameId);
        IGame CreateGame(GameType type);
        bool TryGetGame(Guid gameId, out IGame game);
        int GamesAmount();
    }

    public class GamesController : IGamesController 
    {
        /// <summary>
        /// Games currently running
        /// </summary>
        private Dictionary<Guid, IGame> activeGames = new Dictionary<Guid, IGame>();

        private readonly IGameFactory gamesFactory;
        private readonly ILogger<GamesController> logger;

        public GamesController(
            ILogger<GamesController> logger,
            IGameFactory gamesFactory)
        {
            this.gamesFactory = gamesFactory;
            this.logger = logger;
        }

        public void RemoveGame(Guid gameId)
        {
            if(activeGames.ContainsKey(gameId))
            {
                activeGames.Remove(gameId);
            }
        }

        public IGame CreateGame(GameType type)
        {
            var game = gamesFactory.CreateGame(type);
            activeGames.Add(game.Id, game);

            logger.LogInformation($"Create game id: {game.Id} type: {type}");
            return game;
        }

        public bool TryGetGame(Guid gameId, out IGame game)
        {
            return activeGames.TryGetValue(gameId, out game);
        }

        public int GamesAmount()
        {
            return activeGames.Count;
        }
    }
}
