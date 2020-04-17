using Autofac;
using Magazine.Application;
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

            builder.RegisterType<LogInViewModel>().As<ILogInViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SignUpViewModel>().As<ISignUpViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<NewArticleViewModel>().As<INewArticleViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleListViewModel>().As<IArticleListViewModel>().InstancePerLifetimeScope();

            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("TestDB").Options)).As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<HashProvider>().As<IHashProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleValidateProvider>().As<IArticleValidateProvider>().InstancePerLifetimeScope();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
        }
    }
}
