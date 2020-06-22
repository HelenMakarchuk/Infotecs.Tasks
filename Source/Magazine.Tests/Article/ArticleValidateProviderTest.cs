using Infotecs.Magazine.Domain.Article;
using Infotecs.Magazine.Domain.Contracts.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Xunit;

namespace Infotecs.Magazine.Tests.Article
{
    /// <summary>
    /// Тест валидатора сущности "Статья" <see cref="ArticleValidateProvider"/>.
    /// </summary>
    public class ArticleValidateProviderTest : IClassFixture<TestClassFixture>, IDisposable
    {
        TestClassFixture _testClassFixture;
        IServiceScope _serviceScope;
        ArticleValidateProvider _articleValidateProvider;

        /// <summary>
        /// Выполнение перед каждым тестом.
        /// </summary>
        public ArticleValidateProviderTest(TestClassFixture testClassFixture)
        {
            _testClassFixture = testClassFixture;
            _serviceScope = _testClassFixture.ServiceProvider.CreateScope();
            _articleValidateProvider = (ArticleValidateProvider)_serviceScope.ServiceProvider.GetRequiredService(typeof(IValidateProvider<Domain.Article.Article>));
        }

        /// <summary>
        /// Выполнение после каждого теста.
        /// </summary>
        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        /// <summary>
        /// Выброс исключения если длина контента статьи меньше 2000 символов.
        /// </summary>
        /// <param name="body">Контент статьи.</param>
        [Theory]
        [InlineData("some text")]
        [InlineData(" ")]
        [InlineData("")]
        public void ValidateBody_LengthLessThan2000_ThrowsArgumentException(string body)
        {
            // Assert
            var exception = Assert.Throws<ArgumentException>(() => _articleValidateProvider.ValidateBody(body));
            Assert.Equal(exception.Message, "Body must be at least 2000 characters");
        }

        /// <summary>
        /// Выброс исключения если длина контента превышает 60000 символов.
        /// </summary>
        /// <param name="body">Контент статьи.</param>
        [Fact]
        public void ValidateBody_LengthMoreThan60000_ThrowsArgumentException()
        {
            // Arrange
            string body = new StringBuilder().Append('a', 60001).ToString();

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => _articleValidateProvider.ValidateBody(body));
            Assert.Equal(exception.Message, "Body maximum length exceeded");
        }

        /// <summary>
        /// Выброс исключения если заголовок не указан или содержит только пробелы.
        /// </summary>
        /// <param name="title">Заголовок статьи.</param>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("       ")]
        public void ValidateTitle_EmptyOrWhiteSpace_ThrowsArgumentException(string title)
        {
            // Assert
            var exception = Assert.Throws<ArgumentException>(() => _articleValidateProvider.ValidateTitle(title));
            Assert.Equal(exception.Message, "Title is missing");
        }

        /// <summary>
        /// Выброс исключения если длина заголовка превышает 80 символов.
        /// </summary>
        /// <param name="title">Заголовок статьи.</param>
        [Fact]
        public void ValidateTitle_LengthMoreThan80_ThrowsArgumentException()
        {
            // Arrange
            string title = new StringBuilder().Append('a', 81).ToString();

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => _articleValidateProvider.ValidateTitle(title));
            Assert.Equal(exception.Message, "Title maximum length exceeded");
        }
    }
}
