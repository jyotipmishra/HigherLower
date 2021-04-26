namespace HigherOrLower.Api
{
    using System;
    using System.Threading;
    using HigherOrLower.Infrastructure.DbContexts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            RunDbMigration(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void RunDbMigration(IHost host)
		{
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<HigherOrLowerDbContext>();

                var retriesRemaining = 5;
                do
                {
                    try
                    {
                        //To Allow database to be warmed up and ready. Not so good idea! :-(
                        Thread.Sleep(5000);

                        db.Database.Migrate();

                        retriesRemaining = 0;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(1000);

                        retriesRemaining--;
                        if (retriesRemaining == 0)
                        {
                            throw;
                        }
                    }
                } while (retriesRemaining > 0);
            };
        }
    }
}
