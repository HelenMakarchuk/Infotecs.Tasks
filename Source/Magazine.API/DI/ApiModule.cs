using Autofac;
using Infotecs.Magazine.API.Services;
using Magazine.API.Endpoints;
using Magazine.Application.Contracts.Provider;
using Magazine.Application.Providers;
using Magazine.Infrastracture.Contracts.Repository;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Magazine.Infrastracture.DB;
using Magazine.Infrastracture.DB.Repositories;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Magazine.API.DI
{
    /// <summary>
    /// Модуль контейнера зависимостей для API приложения.
    /// </summary>
    class ApiModule : Module
    {
        IConfiguration _configuration;

        public ApiModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();

            builder.RegisterType<RabbitMqServerEndpoint>().As<RabbitMqServerEndpoint>().SingleInstance();

            builder.RegisterType<HashProvider>().As<IHashProvider>().SingleInstance();

            builder.RegisterType<ArticleService>().As<ArticleService>().SingleInstance();
            builder.RegisterType<CommentService>().As<CommentService>().SingleInstance();
            builder.RegisterType<AccountService>().As<AccountService>().SingleInstance();

            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>()
                .UseNpgsql(_configuration.GetConnectionString("InfotecsMagazine")).Options))
                .As<DbContext>().InstancePerLifetimeScope();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().SingleInstance();
        }
    }
}
