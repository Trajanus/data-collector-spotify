using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollectorSpotify
{
    public class Program
    {
        public static int Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            Collector collector = host.Services.GetService<Collector>();
            DataCollectorSpotifyOptions options = host.Services.GetService<DataCollectorSpotifyOptions>();
            host.Start();

            Uri loginRequestUri = collector.GetLoginRequestUri(options.SpotifyAuthCallbackUri);

            System.Diagnostics.Process.Start(options.BrowserPath, loginRequestUri + " --new-window");

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
