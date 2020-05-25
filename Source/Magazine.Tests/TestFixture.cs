using Autofac;
using Core.DI;
using Infotecs.Magazine.Tests.DI;
using System;

namespace Infotecs.Magazine.Tests
{
    /// <summary>
    /// Класс конфигурации тестового класса.
    /// </summary>
    public class TestFixture : IDisposable
    {
        /// <summary>
        /// Выполнение один раз перед выполнением одного тестового класса.
        /// </summary>
        public TestFixture()
        {
            Container = AutofacConfig.Configure(new TestModule());
        }

        /// <summary>
        /// Выполнение один раз после выполнения одного тестового класса.
        /// </summary>
        public void Dispose()
        {
            Container.Dispose();
        }

        public IContainer Container { get; private set; }
    }
}
