using Infotecs.Magazine.API.ClientCommunicationService.Services;
using Infotecs.Magazine.Domain.Article;
using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Domain.Providers;
using Infotecs.Magazine.Infrastracture.Contracts;
using Infotecs.Magazine.Infrastracture.DB;
using Infotecs.Magazine.Infrastracture.DB.Article;
using Infotecs.Magazine.Infrastracture.DB.Comment;
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
            services.AddSingleton<IValidateProvider<Domain.Article.Article>, ArticleValidateProvider>();
            services.AddSingleton<IValidateProvider<Domain.Comment.Comment>, CommentValidateProvider>();
            services.AddSingleton<IEntityService<Domain.Article.Article>, ArticleService>();
            services.AddSingleton<IEntityService<Domain.Comment.Comment>, CommentService>();

            services.AddSingleton<DbContext>(serviceProvider => new Context(new DbContextOptionsBuilder<Context>()
                     .UseNpgsql(Configuration.GetConnectionString("InfotecsMagazine")).Options));

            services.AddControllers();
            services.AddCors();

            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5082";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200", "http://localhost:5084")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapHub<SignalrService>("/сommunication");
            });
        }
    }
}
