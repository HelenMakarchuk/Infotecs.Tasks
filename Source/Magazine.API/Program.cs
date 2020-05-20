using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Magazine.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
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

                Log.Fatal(ex, "Infotecs Magazine Web API terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .UseStartup<Startup>();
    }
}
