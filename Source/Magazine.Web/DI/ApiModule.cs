using Autofac;
using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Infotecs.Magazine.Infrastracture.DB.Services;
using Magazine.Domain.Entities;
using Magazine.Domain.Providers;
using Magazine.Infrastracture.DB;
using Magazine.Infrastracture.DB.Repositories;
using Magazine.Infrastracture.DB.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Magazine.Web.DI
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
            builder.RegisterType<ArticleValidateProvider>().As<ArticleValidateProvider>().SingleInstance();
            builder.RegisterType<ArticleService>().As<IEntityService<Article>>().SingleInstance();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(Repository<>)).SingleInstance();

            builder.RegisterType<UnitOfWork>().As<UnitOfWork>().InstancePerLifetimeScope();

            builder.Register((c, p) => (DbContext)new Context(new DbContextOptionsBuilder<Context>()
                    .UseNpgsql(_configuration.GetConnectionString("InfotecsMagazine")).Options))
                    .As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
