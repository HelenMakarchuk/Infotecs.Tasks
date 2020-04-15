using Autofac;
using Magazine.Infrastracture.DB;
using Microsoft.EntityFrameworkCore;

namespace Magazine.Tests.DI
{
    class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) => new Context(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("").Options)).As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
