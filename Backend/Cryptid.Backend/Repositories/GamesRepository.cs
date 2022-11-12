using Backend.Data;
using Backend.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Backend.Repositories
{
    public class GamesRepository
    {
        private ApplicationDbContext _context;

        public GamesRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task SaveGame(Game game)
        {
            var existingGame = await _context.Games.FirstOrDefaultAsync(g => g.Id == game.Id);
            if (existingGame == null)
            {
                await _context.Games.AddAsync(game);
            }
            else
            {
                //TODO: Update the game
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteGame(Guid gameId)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
            if(game == null)
            {
                return;
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<Game> GetGame(Guid id)
        {
            var p = await _context.Games.Where(p => p.Id == id).FirstOrDefaultAsync();
            return p;
        }

        public async Task<Player> UpdatePlayer(Player player)
        {
            var p = await _context.Players.Where(p => p.Id == player.Id).FirstOrDefaultAsync();
            p.Nickname = player.Nickname;
            p.Role = player.Role;

            await _context.SaveChangesAsync();

            return p;
        }

        public async Task<List<Player>> GetAllUsers()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<List<Game>> GetAllGamesForPlayer(Guid playerId)
        {
            var games = _context.Games;
            var p = await games.ToListAsync();
            return p;
        }

        public async Task<Player> CreatePlayerByAuthenticationId(string playerId)
        {
            var p = new Player();
            p.AuthenticationId = playerId;
            await _context.Players.AddAsync(p);
            await _context.SaveChangesAsync();

            return p;
        }

        public async Task<Player> SetPlayerName(Guid playerId, string nickname)
        {
            var p = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
            if (_context.Players.FirstOrDefaultAsync(p => p.Nickname == nickname) == null)
            {
                p.Nickname = nickname;
            }

            return p;
        }

        public async Task AddGameParticipants(Guid id, string playerId, string player)
        {
            //await _context.GameParticipants.AddAsync(new GamePlayer(id, player, 2));
            //await _context.GameParticipants.AddAsync(new GamePlayer(id, playerId, 2));
            await _context.SaveChangesAsync();
        }
    }
}
