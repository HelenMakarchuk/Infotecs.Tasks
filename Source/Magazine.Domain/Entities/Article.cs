using Magazine.Domain.Contracts.Entity;
using System.Collections.Generic;

namespace Magazine.Domain.Entities
{
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

        public int Id { get; set; }

        public string Title { get; set; }

        public byte[] Teaser { get; set; }

        public string Body { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}