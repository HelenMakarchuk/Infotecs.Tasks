using Infotecs.Magazine.Domain.Contracts.Provider;
using Infotecs.Magazine.Domain.Entities;
using System;

namespace Infotecs.Magazine.Domain.Providers
{
    /// <summary>
    /// Валидатор сущности "Статья".
    /// </summary>
    public class ArticleValidateProvider : IValidateProvider<Article>
    {
        /// <summary>
        /// Валидация статьи.
        /// </summary>
        /// <param name="article">Экземпляр статьи.</param>
        public void Validate(Article article)
        {
            ValidateTitle(article.Title);
            ValidateBody(article.Body);
        }

        /// <summary>
        /// Валидация контента статьи.
        /// </summary>
        /// <param name="body">Контент статьи.</param>
        /// <exception cref="ArgumentException">
        /// Длина контента менее 2000 символов.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Длина контента больше 60000 символов.
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
        /// Длина заголовка больше 80 символов.
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
