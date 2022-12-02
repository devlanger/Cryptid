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


        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
