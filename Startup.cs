using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using hwmvc.Services;
using hwmvc.Services.Interfaces;
using System;

namespace hwmvc
{
    public class Startup
    {
        private const string NOT_FOUND_PATH = "not-found";
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Enable session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.CookieName = ".hwmvc.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            // For configuration dependency
            services.AddSingleton<IConfiguration>(sp => Configuration);

            // Register services
            services.AddScoped<IWebComicsService, WebComicsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            // Quick an simple way to hande a custom 404
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = $"/{NOT_FOUND_PATH}";
                    await next();
                }
            });

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                /* Tbh I Prefer attribute routing but
                    since this was already defined let's use this :)*/

                routes.MapRoute(
                    name: "not_found",
                    template: NOT_FOUND_PATH,
                    defaults: new { controller = "Home", action = "NotFoundPage" }
                );

                routes.MapRoute(
                    name: "comic_detail",
                    template: "{controller=comic}/{id:int}",
                    defaults: new { action = "Detail" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
