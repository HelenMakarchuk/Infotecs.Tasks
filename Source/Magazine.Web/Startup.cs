using Autofac;
using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Infotecs.Magazine.Infrastracture.DB.Services;
using Magazine.Domain.Entities;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.DB;
using Magazine.Infrastracture.DB.Repositories;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Magazine.Web
{
    public class Startup
    {
        public ILifetimeScope Container { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DbContext>(serviceProvider => new Context(new DbContextOptionsBuilder<Context>()
                    .UseNpgsql(Configuration.GetConnectionString("InfotecsMagazine")).Options));

            services.AddSingleton(typeof(Repository<>), typeof(Repository<>));
            services.AddSingleton<UnitOfWork, UnitOfWork>();
            services.AddSingleton<ArticleValidateProvider, ArticleValidateProvider>();
            services.AddSingleton<IEntityService<Article>, ArticleService>();

            services.AddOptions();

            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSerilogRequestLogging();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
