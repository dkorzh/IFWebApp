using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IF.APM.Client.Direct;
using IF.APM.OpenTelemetry.Direct.Trace;
using OpenTelemetry.Trace;

namespace IFWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddOpenTelemetryTracing(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddFusionExporter(fusionOptions =>
                    {
                        fusionOptions.DirectConnection = new DirectConnectionInformation
                        {
                            Name = "Tutorial",
                            Uri = new Uri("FUSIONHUBURI"),
                            Tls = false, //set to 'true' in production
                            IgnoreTlsErrors = true, //set to 'false' in production
                            UserName = "YOURUSERNAME",
                            Password = "YOURPASSWORD",
                            Exchange = "YOUREXCHANGE"
                        };
                    })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
