using Infotecs.Magazine.Domain.Article;
using Infotecs.Magazine.Domain.Contracts.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infotecs.Magazine.Tests
{
    /// <summary>
    /// Класс конфигурации тестового класса.
    /// </summary>
    public class TestClassFixture : IDisposable
    {
        /// <summary>
        /// Выполнение один раз перед выполнением одного тестового класса.
        /// </summary>
        public TestClassFixture()
        {
            var services = new ServiceCollection();
            services.AddScoped<IValidateProvider<Domain.Article.Article>, ArticleValidateProvider>();
            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Выполнение один раз после выполнения одного тестового класса.
        /// </summary>
        public void Dispose()
        {
            ServiceProvider.Dispose();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}
