using Magazine.Domain.Contracts.Entity;

namespace Magazine.Domain.Entities
{
    /// <summary>
    /// Модель БД сущности "Комментарий".
    /// </summary>
    public class Comment : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Текст комментария.
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// Идентификатор статьи, к которой принадлежит комментарий.
        /// </summary>
        public virtual int ArticleId { get; set; }

        /// <summary>
        /// Идентификатор автора комментария.
        /// </summary>
        public virtual int AccountId { get; set; }

        /// <summary>
        /// Статья, к которой принадлежит комментарий.
        /// </summary>
        public virtual Article Article { get; set; }

        /// <summary>
        /// Автор комментария.
        /// </summary>
        public virtual Account Account { get; set; }
    }
}