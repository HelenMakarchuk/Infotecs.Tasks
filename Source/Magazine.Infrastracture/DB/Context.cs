using Infotecs.Magazine.Infrastracture.DB.Account;
using Infotecs.Magazine.Infrastracture.DB.Article;
using Infotecs.Magazine.Infrastracture.DB.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infotecs.Magazine.Infrastracture.DB
{
    /// <summary>
    /// Контекст основной БД приложения
    /// </summary>
    public class Context : DbContext
    {
        //readonly ILogger _logger;

        /// <summary>
        /// Конструктор требуется при добавлении миграции базы данных
        /// </summary>
        public Context()
            : base()
        { }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            var migrator = Database.GetService<IMigrator>();
            var pendingMigrations = Database.GetPendingMigrations();

            foreach (var targetMigration in pendingMigrations)
            {
                migrator.Migrate(targetMigration);
            }
        }

        public virtual DbSet<Domain.Article.Article> Articles { get; set; }
        public virtual DbSet<Domain.Account.Account> Users { get; set; }
        public virtual DbSet<Domain.Comment.Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
        }

        /// <summary>
        /// Метод требуется при добавлении миграции базы данных
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=InfotecsMagazine;User Id=postgres;Password=1;");
            }
        }
    }
}
