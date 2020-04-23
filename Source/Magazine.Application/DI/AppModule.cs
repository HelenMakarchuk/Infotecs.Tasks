using Autofac;
using Infotecs.Magazine.Application.Contracts.ViewModel;
using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Application.ViewModels;
using Magazine.Application.Contracts.ViewModel;
using Magazine.Application.Pages;
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

            builder.RegisterType<ApplicationViewModel>().As<IApplicationViewModel>().SingleInstance();
            builder.RegisterType<LogInViewModel>().As<ILogInViewModel>().SingleInstance();
            builder.RegisterType<SignUpViewModel>().As<ISignUpViewModel>().SingleInstance();
            builder.RegisterType<NewArticleViewModel>().As<INewArticleViewModel>().SingleInstance();
            builder.RegisterType<ArticleListViewModel>().As<IArticleListViewModel>().SingleInstance();

            builder.RegisterType<RabbitMqClientEndpoint>().As<RabbitMqClientEndpoint>().SingleInstance();

            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>()
             .UseNpgsql(ConfigurationManager.ConnectionStrings["InfotecsMagazine"]?.ConnectionString).Options))
             .As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();

            builder.RegisterType<ArticleValidateProvider>().As<IArticleValidateProvider>().SingleInstance();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().SingleInstance();
        }
    }
}