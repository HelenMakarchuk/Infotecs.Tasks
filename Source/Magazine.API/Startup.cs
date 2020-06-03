using Infotecs.Magazine.API.ClientCommunicationService.Services;
using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Domain.Providers;
using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Infotecs.Magazine.Infrastracture.DB;
using Infotecs.Magazine.Infrastracture.DB.Repositories;
using Infotecs.Magazine.Infrastracture.DB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infotecs.Magazine.API
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
            services.AddSingleton<IValidateProvider<Domain.Entities.Article>, ArticleValidateProvider>();
            services.AddSingleton<IValidateProvider<Domain.Entities.Comment>, CommentValidateProvider>();
            services.AddSingleton<IEntityService<Domain.Entities.Article>, ArticleService>();
            services.AddSingleton<IEntityService<Domain.Entities.Comment>, CommentService>();

            services.AddSingleton<DbContext>(serviceProvider => new Context(new DbContextOptionsBuilder<Context>()
                     .UseNpgsql(Configuration.GetConnectionString("InfotecsMagazine")).Options));

            services.AddControllers();
            services.AddCors();

            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalrService>("/сommunication");
            });
        }
    }
}
