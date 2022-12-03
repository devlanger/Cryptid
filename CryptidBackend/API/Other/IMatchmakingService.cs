﻿using Application.Games;
using Backend.Logic;
using Cryptid.Backend.Hubs;
using Cryptid.Shared.Logic;
using CryptidClient.Assets.Scripts.MapLoader;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence.Data;
using Persistence.Repositories;

namespace Cryptid.Backend
{
    public class MatchmakingService
    {
        private readonly IHubContext<GameHub> hub;
        private readonly DataContext context;

        // private readonly IGamesController gamesController;
        private readonly ILogger<MatchmakingService> logger;
        private readonly CacheRepository cacheRepository;
        private readonly UserManager<AppUser> userManager;

        public MatchmakingService(
            ILogger<MatchmakingService> logger,
            IHubContext<GameHub> hub,
            DataContext context,
            CacheRepository cacheRepository,
            UserManager<AppUser> userManager)
        {
            this.logger = logger;
            this.hub = hub;
            this.context = context;
            this.cacheRepository = cacheRepository;
            this.userManager = userManager;
        }

        public void AddPlayerToMatchmaking(string id)
        {
            cacheRepository.AddPlayerToMatchmaking(id);
        }

        public void RemovePlayerMatchmaking(string id)
        {
            cacheRepository.RemovePlayerMatchmaking(id);
        }

        public async Task MatchPlayers()
        {
             logger.LogInformation($"Matchmaking update {DateTimeOffset.Now} Players waiting: {cacheRepository.WaitingPlayers.Count}");

            string lastPlayer = "";
            foreach (var player in cacheRepository.WaitingPlayers.ToList())
            {
                if(string.IsNullOrEmpty(lastPlayer))
                {
                    lastPlayer = player;
                }
                else
                {
                    await MatchSelectedPlayers(lastPlayer, player);
                    cacheRepository.RemovePlayerMatchmaking(lastPlayer);
                    cacheRepository.RemovePlayerMatchmaking(player);
                }
            }
        }

        private async Task MatchSelectedPlayers(string playerId, string player)
        {
            logger.LogInformation($"Log player {playerId} with player {player}");
            
            var g = await context.Games.FindAsync(GameHub.GLOBAL_GAME_ID);
            GameState gameState;

            if (g == null)
            {
                var game = new GameFactory().CreateGame(GameType.TEAM) as Game;
                var gameResult = await context.Games.AddAsync(game);

                gameState = GameInstanceFactory.StartNewGame(new GameStartSettings
                {
                    players = 3
                });

                game.CurrentState = GameStateSerializationHelper.Save(gameState);
                game.Participants = new List<GameParticipant>();
                var user = await userManager.FindByEmailAsync("bob@test.com");
                game.Participants.Add(new GameParticipant()
                {
                    AppUser = user,
                    Game = game
                });
                await context.SaveChangesAsync();
            }
            else
            {
                gameState = GameStateSerializationHelper.Load<GameState>(g.CurrentState);
            }

            var gameStateJson = JsonConvert.SerializeObject(gameState);
            await hub.Clients.Client(playerId).SendAsync("LoadGameState", gameStateJson);
            await hub.Clients.Client(player).SendAsync("LoadGameState", gameStateJson);
        }
    }
}
