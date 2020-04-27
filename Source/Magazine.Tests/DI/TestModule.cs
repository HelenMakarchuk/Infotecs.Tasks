using Autofac;
using Magazine.Desktop;
using Magazine.Desktop.Contracts.Provider;
using Magazine.Desktop.Pages;
using Magazine.Desktop.Providers;
using Magazine.Desktop.ViewModels;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.Contracts.Repository;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Magazine.Infrastracture.DB;
using Magazine.Infrastracture.DB.Repositories;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Magazine.Tests.DI
{
    /// <summary>
    /// Модуль контейнера зависимостей для тестов.
    /// </summary>
    class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationWindow>().As<ApplicationWindow>().InstancePerLifetimeScope();

            builder.RegisterType<LogInPage>().As<LogInPage>().InstancePerLifetimeScope();
            builder.RegisterType<SignUpPage>().As<SignUpPage>().InstancePerLifetimeScope();
            builder.RegisterType<NewArticlePage>().As<NewArticlePage>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleListPage>().As<ArticleListPage>().InstancePerLifetimeScope();

            builder.RegisterType<LogInViewModel>().As<LogInViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SignUpViewModel>().As<SignUpViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<NewArticleViewModel>().As<NewArticleViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleListViewModel>().As<ArticleListViewModel>().InstancePerLifetimeScope();

            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("TestContext").Options)).As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<HashProvider>().As<IHashProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleValidateProvider>().As<ArticleValidateProvider>().InstancePerLifetimeScope();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().InstancePerLifetimeScope();
        }
    }
}
