using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Reflection.Emit;

namespace Persistence.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameParticipant> Participants { get; set; }


        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GameParticipant>(g => g
                .HasKey(gu => new
                {
                    gu.AppUserId,
                    gu.GameId
                }));

            builder.Entity<GameParticipant>()
                .HasOne(u => u.AppUser)
                .WithMany(g => g.Games)
                .HasForeignKey(gu => gu.AppUserId);

            builder.Entity<GameParticipant>()
                .HasOne(u => u.Game)
                .WithMany(g => g.Participants)
                .HasForeignKey(gu => gu.GameId);
        }
    }
}
