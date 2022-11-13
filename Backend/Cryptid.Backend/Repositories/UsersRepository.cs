using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Xml.Linq;

namespace Backend.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Player> GetUserData(string name)
        {
            var p = await _context.Players.Where(p => p.Nickname == name).FirstOrDefaultAsync();
            return p;
        }

        public async Task<Player> GetPlayer(Guid id)
        {
            var p = await _context.Players.Where(p => p.Id == id).FirstOrDefaultAsync();
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

        public async Task<Player> CreateOrLoginPlayer(string userId)
        {
            var existing = _context.Players.FirstOrDefaultAsync(p => p.AuthenticationId == userId);
            if (existing.Result != null)
            {
                var p = existing.Result;
                p.LastLoginTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return p;
            }
            else
            {
                var p = new Player()
                {
                    Id = Guid.NewGuid(),
                    Nickname = "Test",
                    AuthenticationId = userId,
                    Role = "user"
                };

                await _context.Players.AddAsync(p);
                await _context.SaveChangesAsync();

                return p;
            }
        }
    }
}
