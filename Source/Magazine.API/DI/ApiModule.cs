using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infotecs.Magazine.Infrastracture.Endpoints;
using Magazine.API.Endpoints;
using Magazine.Application.Contracts.Provider;
using Magazine.Application.Contracts.Service;
using Magazine.Application.Providers;
using Magazine.Application.Services;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.DB.EntityConfigurations;
using Microsoft.Extensions.Configuration;
using NHibernate;
using Serilog;
using System.Configuration;

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
            builder.RegisterType<App>().As<App>().SingleInstance();

            builder.RegisterType<WpfEndpoint>().As<RabbitMQEndpoint>().SingleInstance();

            builder.RegisterType<HashProvider>().As<IHashProvider>().SingleInstance();
            builder.RegisterType<ArticleValidateProvider>().As<IArticleValidateProvider>().SingleInstance();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();

            builder.Register((c, p) => Fluently.Configure()
                .Database
                (
                    PostgreSQLConfiguration.Standard
                    .ConnectionString(ConfigurationManager.ConnectionStrings["InfotecsMagazine"]?.ConnectionString)
                )
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<ArticleMap>();
                    m.FluentMappings.AddFromAssemblyOf<AccountMap>();
                    m.FluentMappings.AddFromAssemblyOf<CommentMap>();
                })
                .BuildSessionFactory()).As<ISessionFactory>().SingleInstance();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().SingleInstance();
        }
    }
}
