using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using API.Interfaces;
using API.Controllers.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Extensions;
using GraphQL.Server.Ui.Playground;
using API.GraphQL;

namespace API
{
    public class Startup
    {
        private readonly IWebHostEnvironment WebHostEnvironment;
        public IHostEnvironment HostEnvironment { get; }
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
            WebHostEnvironment = webHostEnvironment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(Configuration);
            //services.AddControllers();
            services.AddCors();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //services.AddSwaggerGen(c =>
            //{
              //  c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
          //  {
            //    app.UseDeveloperExceptionPage();
              //  app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
           // app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseGraphQL("/graphql");
            app.UseGraphQL<AppUserSchema>();
            app.UseGraphQLPlayground("/", new PlaygroundOptions());
            //app.UseEndpoints(endpoints =>
           // {
             //   endpoints.MapControllers();
            //});
        }
    }
}
