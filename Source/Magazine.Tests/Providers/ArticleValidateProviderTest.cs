using Autofac;
using Magazine.Domain.Contracts.Provider;
using System;
using System.Text;
using Xunit;

namespace Infotecs.Magazine.Tests.Providers
{
    /// <summary>
    /// Тест валидатора сущности "Статья" <see cref="ArticleValidateProvider"/>.
    /// </summary>
    public class ArticleValidateProviderTest : IClassFixture<TestFixture>, IDisposable
    {
        TestFixture _testFixture;
        ILifetimeScope _containerScope;
        IArticleValidateProvider _articleValidateProvider;

        /// <summary>
        /// Выполнение перед каждым тестом.
        /// </summary>
        public ArticleValidateProviderTest(TestFixture testFixture)
        {
            _testFixture = testFixture;
            _containerScope = _testFixture.Container.BeginLifetimeScope();
            _articleValidateProvider = _testFixture.Container.Resolve<IArticleValidateProvider>();
        }

        /// <summary>
        /// Выполнение после каждого теста.
        /// </summary>
        public void Dispose()
        {
            _containerScope.Dispose();
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
