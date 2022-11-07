using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Web.Mvc;
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

        public string GetUserData(string name)
        {
            var p = _context.Players.Where(p => p.Nickname == name).FirstOrDefault();
            if(p == null)
            {
                return "";
            }

            return p.Nickname;
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

        public void GetUserData()
        {
            throw new NotImplementedException();
        }
    }
}
