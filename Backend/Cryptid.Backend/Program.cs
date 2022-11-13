using Backend.Data;
using Backend.Logic;
using Backend.Repositories;
using Cryptid.Backend;
using Cryptid.Backend.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Cryptid.Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connPath = "DefaultConnection";
var connectionString = builder.Configuration.GetConnectionString(connPath);

string? conn = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
if (conn != null)
{
    connectionString = conn;
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Host.ConfigureLogging(logging =>
{
    //logging.ClearProviders();
    logging.AddConsole(); 
});


builder.Services.AddHostedService<CryptidBackendService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<CacheRepository>();
builder.Services.AddScoped<IGameFactory, GameFactory>();
builder.Services.AddScoped<MatchmakingService>();
builder.Services.AddScoped<IGamesController, GamesController>();
builder.Services.AddScoped<GamesRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSignalR();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();

//Add Google OAuth
//builder.Services.AddAuthentication().AddGoogle(googleOptions => {
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.UseDeveloperExceptionPage();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}


app.UseHttpsRedirection();

app.MapHub<GameHub>("/gameHub");
app.MapGet("/", () => VersionManager.GitHash);

app.Run();
