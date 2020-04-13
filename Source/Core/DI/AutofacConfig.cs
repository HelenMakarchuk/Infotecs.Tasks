using Autofac;
using System;

namespace Core.DI
{
    public static class AutofacConfig
    {
        public static IContainer Configure(params Module[] modules)
        {
            var builder = new ContainerBuilder();
            Array.ForEach(modules, m => builder.RegisterModule(m));
            return builder.Build();
        }
    }
}
