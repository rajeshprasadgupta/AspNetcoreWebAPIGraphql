using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Services;
using API.Data;
using API.GraphQL;
using API.Helpers;
using API.Interfaces;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using GraphQL.Server.Transports.AspNetCore.SystemTextJson;
namespace API.Extensions
{
    public static class ApplicationServicesExtenstion
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration configuration){
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<AppUserQuery>();
            services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options => {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<AppUserSchema>();
            services.AddGraphQL(builder => builder.AddSystemTextJson()
            //.AddSchema<AppUserSchema>()
            );
            return services;
        }
    }
}