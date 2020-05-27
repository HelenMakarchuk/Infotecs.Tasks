using Infotecs.Magazine.Domain.Contracts.Entity;

namespace Infotecs.Magazine.Domain.Entities
{
    /// <summary>
    /// Модель БД сущности "Комментарий".
    /// </summary>
    public class Comment : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Текст комментария.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Идентификатор статьи, к которой принадлежит комментарий.
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// Идентификатор автора комментария.
        /// </summary>
        public int AccountId { get; set; }
    }
}
