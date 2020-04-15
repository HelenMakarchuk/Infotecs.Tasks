using Magazine.Domain.Contracts.Entity;

namespace Magazine.Domain.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public int ArticleId { get; set; }

        public int UserId { get; set; }

        public virtual Article Article { get; set; }
        public virtual User User { get; set; }
    }
}