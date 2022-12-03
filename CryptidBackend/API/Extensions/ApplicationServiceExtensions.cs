using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Backend.Logic;
using Cryptid.Backend;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Persistence.Repositories;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            string connPath = "DefaultConnection";
            var connectionString = config.GetConnectionString(connPath);

            string? conn = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
            if (conn != null)
            {
                connectionString = conn;
            }

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(connectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });

            services.AddCors(opts => {
                opts.AddPolicy("CorsPolicy", policy => 
                {
                    //policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });

            services.AddHostedService<CryptidBackendService>();
            services.AddMemoryCache();
            services.AddScoped<CacheRepository>();
            services.AddScoped<IGameFactory, GameFactory>();
            services.AddScoped<MatchmakingService>();
            services.AddScoped<AuthService>();
            services.AddScoped<ActionsController>();
            services.AddSignalR();

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddMediatR(typeof(Application.Games.List.Query).Assembly);

            return services;
        }
    }
}