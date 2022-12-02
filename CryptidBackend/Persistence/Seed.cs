using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>()
                {
                    new AppUser{Nickname="Bob", Email = "bob@test.com", UserName="bob"},
                    new AppUser{Nickname="Tom", Email = "tom@test.com", UserName="tom"},
                    new AppUser{Nickname="Rob", Email = "rob@test.com", UserName="rob"},
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$word");
                }
            }

            if(!context.Games.Any())
            {
                var games = new List<Game>
                {
                    new Game { },
                    new Game { },
                    new Game { },
                };

                await context.Games.AddRangeAsync(games);
                await context.SaveChangesAsync();
            }
        }
    }
}