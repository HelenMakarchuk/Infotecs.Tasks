using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Magazine.Application.Contracts.Provider;
using Magazine.Application.Contracts.Service;
using Magazine.Application.Contracts.ViewModel;
using Magazine.Application.Pages;
using Magazine.Application.Providers;
using Magazine.Application.Services;
using Magazine.Application.ViewModels;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.DB.EntityConfigurations;
using NHibernate;
using Serilog;
using System.Configuration;

namespace Magazine.Application.DI
{
    /// <summary>
    /// Модуль контейнера зависимостей для приложения.
    /// </summary>
    class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationWindow>().As<ApplicationWindow>().SingleInstance();

            builder.RegisterType<LogInPage>().As<LogInPage>().SingleInstance();
            builder.RegisterType<SignUpPage>().As<SignUpPage>().SingleInstance();
            builder.RegisterType<NewArticlePage>().As<NewArticlePage>().SingleInstance();
            builder.RegisterType<ArticleListPage>().As<ArticleListPage>().SingleInstance();

            builder.RegisterType<LogInViewModel>().As<ILogInViewModel>().SingleInstance();
            builder.RegisterType<SignUpViewModel>().As<ISignUpViewModel>().SingleInstance();
            builder.RegisterType<NewArticleViewModel>().As<INewArticleViewModel>().SingleInstance();
            builder.RegisterType<ArticleListViewModel>().As<IArticleListViewModel>().SingleInstance();

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
                    m.FluentMappings.AddFromAssemblyOf<UserMap>();
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