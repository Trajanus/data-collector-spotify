using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Autofac;
using Serilog.Events;
using Serilog.Core;
using System.Reflection;
using Autofac.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DataCollectorSpotify
{
    public class Startup
    {
        const string SettingsFileName = "appsettings.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var provider = services.BuildServiceProvider().GetRequiredService<IActionDescriptorCollectionProvider>();

            var routes = provider.ActionDescriptors.Items.Where(
            ad => ad.AttributeRouteInfo != null).Select(ad => (ad.AttributeRouteInfo.Name, ad.AttributeRouteInfo.Template)).ToList();
            foreach (var route in routes)
            {
                Log.Information($"Route Name: {route.Name} - Route Template: {route.Template}");
            }

        }

        // runs after ConfigureServices
        public void ConfigureContainer(ContainerBuilder builder)
        {
            DataCollectorSpotifyOptions options = ReadConfiguration(SettingsFileName);

            ConfigSerilog(options.SerilogLogEventLevel, options.LogDirectoryPath);

            builder.RegisterType<Collector>().AsSelf().SingleInstance().WithParameter("clientId", options.SpotifyDataCollectorClientId);
            builder.RegisterInstance(options);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;

                Log.Error(exception, "Unhandled exception.");

                var response = new { error = exception.Message };
                await context.Response.WriteAsync(exception.Message);
            }));

            //TODO
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

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
