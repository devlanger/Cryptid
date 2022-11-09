using Backend.Logic;
using Cryptid.Backend;
using Cryptid.Backend.Hubs;
using Microsoft.AspNetCore.Builder;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddSingleton<IGameFactory, GameFactory>();
builder.Services.AddSingleton<IGamesController, GamesController>();
builder.Services.AddSingleton<MatchmakingService>();

builder.Services.AddHostedService<CryptidBackendWorker>();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHub<GameHub>("/gameHub");
app.MapGet("/", () => VersionManager.GitHash);

app.Run();
