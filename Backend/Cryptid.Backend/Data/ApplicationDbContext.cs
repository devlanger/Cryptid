using Backend.Logic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Reflection.Emit;

namespace Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<FriendsPair> Friends { get; set; }
        public DbSet<Game> Games { get; set; }
        //public DbSet<GamePlayer> GameParticipants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
