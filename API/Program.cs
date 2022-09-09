using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //seed data in database for the first time run
            await SeedDataToDatabaseAsync(host);
            await host.RunAsync(); 
        }

        private static async Task SeedDataToDatabaseAsync(IHost host )
        {
            //get service
            using var scope = host.Services.CreateScope();
            var service = scope.ServiceProvider;
            //Catch any exception during seed
            try
            {
                var context = service.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();
                await Seed.SeedAsync(context);
            }
            catch(Exception ex){
                    var logger = service.GetRequiredService<ILogger>();
                    logger.LogError(ex, "Error Occuring during data seed");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
