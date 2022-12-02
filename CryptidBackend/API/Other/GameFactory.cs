using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Logic
{
    public class GameFactory : IGameFactory
    {
        public IGame CreateGame(GameType type)
        {
            IGame result = null;
            switch (type)
            {
                case GameType.TEAM:
                    result = new TeamGame();
                    break;
                case GameType.DEATHMATCH:
                    break;
                default:
                    break;
            }

            result.Id = Guid.NewGuid();
            return result;
        }
    }

    public interface IGameFactory
    {
        IGame CreateGame(GameType type);
    }
}