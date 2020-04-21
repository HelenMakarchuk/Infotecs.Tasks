using Autofac;
using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Endpoints;
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
using Magazine.Infrastracture.Contracts.Repository;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Magazine.Infrastracture.DB;
using Magazine.Infrastracture.DB.Repositories;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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

            builder.RegisterType<ApiEndpoint>().As<RabbitMQEndpoint>().SingleInstance();

            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>()
             .UseNpgsql(ConfigurationManager.ConnectionStrings["InfotecsMagazine"]?.ConnectionString).Options))
             .As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();

            builder.RegisterType<HashProvider>().As<IHashProvider>().SingleInstance();
            builder.RegisterType<ArticleValidateProvider>().As<IArticleValidateProvider>().SingleInstance();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().SingleInstance();
        }
    }
}