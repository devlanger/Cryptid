using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Logic;
using Cryptid.Backend;
using Cryptid.Backend.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repositories;
using MediatR;
using Domain.Models;
using API.Extensions;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

         public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationServices(config);
            services.AddIdentityServices(config);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameHub>("/gameHub");
                endpoints.MapGet("/", () => VersionManager.GitHash);
                endpoints.MapControllers();
            });
        }
    }
}