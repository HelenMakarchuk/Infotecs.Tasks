using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Infotecs.Magazine.Infrastracture.DB.Services;
using Magazine.Domain.Entities;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.DB;
using Magazine.Infrastracture.DB.Repositories;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Magazine.API
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
            services.AddSingleton(typeof(Repository<>), typeof(Repository<>));
            services.AddSingleton<UnitOfWork, UnitOfWork>();
            services.AddSingleton<ArticleValidateProvider, ArticleValidateProvider>();
            services.AddSingleton<IEntityService<Article>, ArticleService>();

            services.AddSingleton<DbContext>(serviceProvider => new Context(new DbContextOptionsBuilder<Context>()
                     .UseNpgsql(Configuration.GetConnectionString("InfotecsMagazine")).Options));

            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
