using Infotecs.Magazine.Domain.Contracts.Entity;

namespace Infotecs.Magazine.Domain.Entities
{
    /// <summary>
    /// Модель БД сущности "Пользователь".
    /// </summary>
    public class Account : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Соль, которая применяется при создании хеша пароля.
        /// </summary>
        public string Salt { get; set; }
    }
}