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
            services.AddDbContext<Context>(options => options.UseNpgsql(Configuration.GetConnectionString("InfotecsMagazine")), ServiceLifetime.Scoped);
            services.AddScoped(typeof(Repository<>), typeof(Repository<>));
            services.AddScoped<UnitOfWork, UnitOfWork>();
            services.AddScoped<IEntityService<Domain.Article.Article>, ArticleService>();
            services.AddScoped<IEntityService<Domain.Comment.Comment>, CommentService>();
            services.AddScoped<IValidateProvider<Domain.Article.Article>, ArticleValidateProvider>();
            services.AddScoped<IValidateProvider<Domain.Comment.Comment>, CommentValidateProvider>();

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
                    options.Audience = "api";
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
