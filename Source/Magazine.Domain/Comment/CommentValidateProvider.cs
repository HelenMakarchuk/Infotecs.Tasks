using Infotecs.Magazine.Domain.Contracts.Provider;
using System;

namespace Infotecs.Magazine.Domain.Comment
{
    /// <summary>
    /// Валидатор сущности "Комментарий".
    /// </summary>
    public class CommentValidateProvider : IValidateProvider<Comment>
    {
        /// <summary>
        /// Валидация статьи.
        /// </summary>
        /// <param name="comment">Экземпляр статьи.</param>
        public void Validate(Comment comment)
        {
            ValidateBody(comment.Body);
        }

        /// <summary>
        /// Валидация комментария.
        /// </summary>
        /// <param name="body">Контент комментария.</param>
        /// <exception cref="ArgumentException">
        /// Комментарий не указан.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Длина контента больше 6000 символов.
        /// </exception>
        public void ValidateBody(string body)
        {
            if (String.IsNullOrWhiteSpace(body))
                throw new ArgumentException("Body missed");

            if (body.Length > 6000)
                throw new ArgumentException("Body maximum length exceeded");
        }
    }
}
