using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace IdentityServerAspNetIdentity
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                // Конфигурация логирования независимо от основной конфигурации приложения при возникновении исключения при запуске приложения.
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                    Log.Logger = new LoggerConfiguration()
                                         .MinimumLevel.Verbose()
                                         .WriteTo.File("Logs/.log", LogEventLevel.Verbose, rollingInterval: RollingInterval.Hour)
                                         .CreateLogger();

                Log.Fatal(ex, "Infotecs Identity terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
                });
    }
}
