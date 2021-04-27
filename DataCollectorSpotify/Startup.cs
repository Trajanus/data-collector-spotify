using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace DataCollectorSpotify
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
