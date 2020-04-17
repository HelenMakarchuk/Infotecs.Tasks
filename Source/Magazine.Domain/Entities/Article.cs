using Magazine.Domain.Contracts.Entity;
using System.Collections.Generic;

namespace Magazine.Domain.Entities
{
    /// <summary>
    /// Модель БД сущности "Статья".
    /// </summary>
    public class Article : IEntity
    {
        public Article() { }

        public Article(string title, string body, int userId, byte[] teaser = null)
        {
            Title = title;
            Body = body;
            Teaser = teaser;
            UserId = userId;
        }

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Картинка-тизер.
        /// </summary>
        public byte[] Teaser { get; set; }

        /// <summary>
        /// Контент статьи.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Идентификатор автора статьи.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Автор статьи.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Список комментариев к статье.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }
    }
}