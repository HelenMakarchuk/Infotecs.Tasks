using Magazine.Domain.Contracts.Provider;
using Serilog;
using System;

namespace Magazine.Domain.Providers
{
    /// <summary>
    /// Валидатор сущности "Статья".
    /// </summary>
    public class ArticleValidateProvider : IArticleValidateProvider
    {
        ILogger _logger;

        public ArticleValidateProvider(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Валидация контента статьи.
        /// </summary>
        /// <param name="body">Контент статьи.</param>
        /// <exception cref="ArgumentException">
        /// Длина контента менее 2000 символов.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Превышена максимальная длина контента.
        /// </exception>
        public void ValidateBody(string body)
        {
            if (body.Length < 2000)
                throw new ArgumentException("Body must be at least 2000 characters");

            if (body.Length > 60000)
                throw new ArgumentException("Body maximum length exceeded");
        }

        /// <summary>
        /// Валидация заголовка статьи.
        /// </summary>
        /// <param name="title">Заголовок статьи.</param>
        /// <exception cref="ArgumentException">
        /// Заголовок не указан или содержит только пробелы.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Превышена максимальная длина заголовка.
        /// </exception>
        public void ValidateTitle(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is missing");

            if (title.Length > 80)
                throw new ArgumentException("Title maximum length exceeded");
        }
    }
}
