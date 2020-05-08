using Magazine.Domain.Entities;
using Magazine.Infrastracture.DB.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Magazine.Infrastracture.DB
{
    /// <summary>
    /// Контекст основной БД приложения
    /// </summary>
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Account> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
        }
    }
}