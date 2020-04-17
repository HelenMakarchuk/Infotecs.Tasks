using Magazine.Domain.Contracts.Entity;
using System.Collections.Generic;

namespace Magazine.Domain.Entities
{
	/// <summary>
	/// Модель БД сущности "Пользователь".
	/// </summary>
	public class User : IEntity
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