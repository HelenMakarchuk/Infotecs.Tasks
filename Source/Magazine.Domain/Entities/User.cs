using Magazine.Domain.Contracts.Entity;
using System.Collections.Generic;

namespace Magazine.Domain.Entities
{
	public class User : IEntity
	{
		public int Id { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public string Salt { get; set; }

		public virtual ICollection<Article> Articles { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}