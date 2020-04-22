using Magazine.Domain.Contracts.Entity;
using System.Collections.Generic;

namespace Magazine.Domain.Entities
{
    /// <summary>
    /// Модель БД сущности "Пользователь".
    /// </summary>
    public class Account : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Соль, которая применяется при создании хеша пароля.
        /// </summary>
        public virtual string Salt { get; set; }

        /// <summary>
        /// Список статей, автором которых является текущий пользователь.
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }

        /// <summary>
        /// Список комментариев, автором которых является текущий пользователь.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }
    }
}