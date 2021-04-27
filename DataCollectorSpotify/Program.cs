using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Reflection;

namespace DataCollectorSpotify
{
    public class Program
    {
        const string SettingsFileName = "appsettings.json";

        public static int Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DataCollectorSpotifyOptions options = ReadConfiguration(SettingsFileName);

            ConfigSerilog(options.SerilogLogEventLevel, options.LogDirectoryPath);

            CreateHostBuilder(args).Build().Run();

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigSerilog(LogEventLevel logLevel, string logDirectoryPath)
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = logLevel;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logDirectoryPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();

            Log.Information($"Logger for {Assembly.GetEntryAssembly().GetName().Name} started.");
        }

        private static DataCollectorSpotifyOptions ReadConfiguration(string settingsFileName)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(settingsFileName);
            IConfiguration configuration = configurationBuilder.Build();
            var options = new DataCollectorSpotifyOptions();
            configuration.Bind(options);
            return options;
        }
    }
}
