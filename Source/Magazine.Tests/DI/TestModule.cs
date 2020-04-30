using Autofac;
using Infotecs.Magazine.Domain.Providers;
using Magazine.Domain.Providers;
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
            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("TestContext").Options)).As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(Repository<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<UnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<HashProvider>().As<HashProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleValidateProvider>().As<ArticleValidateProvider>().InstancePerLifetimeScope();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().InstancePerLifetimeScope();
        }
    }
}
